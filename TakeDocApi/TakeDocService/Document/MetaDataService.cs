using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.MyUnityHelper;
using DAO = TakeDocDataAccess.Document;

namespace TakeDocService.Document
{
    public class MetaDataService : TakeDocService.BaseService, Interface.IMetaDataService
    {
        DAO.Interface.IDaoMetaData dao = UnityHelper.Resolve<DAO.Interface.IDaoMetaData>();

        Interface.IDataFieldService servDataField = UnityHelper.Resolve<Interface.IDataFieldService>();

        public void CreateMetaData(Guid userId, Guid entityId, Guid documentId, Guid typeDocumentId)
        {
            ICollection<TakeDocModel.DataField> fields = servDataField.GetDataField(typeDocumentId, entityId);
            foreach (TakeDocModel.DataField field in fields)
            {
                TakeDocModel.MetaData meta = new TakeDocModel.MetaData();
                meta.DataFieldId = field.DataFieldId;
                meta.DateCreateData = System.DateTime.UtcNow;
                meta.EntityId = entityId;
                meta.EtatDeleteData = false;
                meta.MetaDataDocumentId = documentId;
                meta.MetaDataId = System.Guid.NewGuid();
                meta.MetaDataName = field.DataFieldReference;
                meta.UserCreateData = userId;

                dao.Add(meta);
            } 
        }

        public void SetMetaData(Guid userId, Guid entityId, Guid documentId, IDictionary<string, string> metadatas)
        {
            ICollection<TakeDocModel.DataField> fields = servDataField.GetDataField(metadatas.Keys, entityId);
            foreach (KeyValuePair<string,string> metadata in metadatas)
            {
                ICollection<TakeDocModel.DataField> field = fields.Where(x => x.DataFieldReference == metadata.Key).ToList();
                if (field.Count() > 0)
                {
                    bool ok = this.IsValid(field.First().DataFieldType, metadata.Value, field.First().DataFieldMandatory);
                    if (ok == false) throw new Exception("MetaData non valide.");
                }
            }
            dao.SetMetaData(userId, entityId, documentId, metadatas);
        }

        public bool IsValid(string typeName, string value, bool required)
        {
            if (string.IsNullOrEmpty(value) == true && required == true) return false;
            Type myType = Type.GetType(typeName);
            try
            {
                if (myType == typeof(System.String))
                {
                    object o1 = Activator.CreateInstance(myType, new object[] { value.ToArray() });
                }
                else if (myType == typeof(System.DateTimeOffset))
                {
                   DateTimeOffset.Parse(value);
                }
                else if (myType == typeof(System.Boolean))
                {
                    Boolean.Parse(value);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
