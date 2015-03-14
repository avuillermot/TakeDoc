using System;
using System.Collections.Generic;
using System.Globalization;
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

        public void CreateMetaData(Guid userId, Guid entityId, Guid versionId, Guid typeDocumentId)
        {
            ICollection<TakeDocModel.View_TypeDocumentDataField> fields = servDataField.GetDataField(typeDocumentId, entityId);
            foreach (TakeDocModel.View_TypeDocumentDataField field in fields)
            {
                TakeDocModel.MetaData meta = new TakeDocModel.MetaData();
                meta.DataFieldId = field.FieldId;
                meta.DateCreateData = System.DateTime.UtcNow;
                meta.EntityId = entityId;
                meta.EtatDeleteData = false;
                meta.MetaDataDisplayIndex = field.DisplayIndex;
                meta.MetaDataMandatory = field.Mandatory;
                meta.MetaDataVersionId = versionId;
                meta.MetaDataId = System.Guid.NewGuid();
                meta.MetaDataName = field.Reference;
                meta.UserCreateData = userId;

                dao.Add(meta);
            } 
        }

        public void SetMetaData(Guid userId, Guid entityId, Guid versionId, IDictionary<string, string> metadatas)
        {
            ICollection<TakeDocModel.View_TypeDocumentDataField> fields = servDataField.GetDataField(metadatas.Keys, entityId);
            foreach (KeyValuePair<string,string> metadata in metadatas)
            {
                ICollection<TakeDocModel.View_TypeDocumentDataField> field = fields.Where(x => x.Reference == metadata.Key).ToList();
                if (field.Count() > 0)
                {
                    bool ok = this.IsValid(field.First().TypeId, metadata.Value, field.First().Mandatory);
                    if (ok == false) throw new Exception("MetaData non valide.");
                }
            }
            dao.SetMetaData(userId, entityId, versionId, metadatas);
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
                if (myType == typeof(System.Int32))
                {
                    int inInt = int.MinValue;
                    int.TryParse(value, out inInt);
                }
                else if (myType == typeof(System.DateTimeOffset))
                {
                   var ci = new CultureInfo("fr-FR");
                   DateTimeOffset.Parse(value, ci.DateTimeFormat);
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

        public ICollection<TakeDocModel.MetaData> GetByVersion(Guid versionId, Guid entityId)
        {
            return dao.GetBy(x => x.MetaDataVersionId == versionId && x.EntityId == entityId && x.EtatDeleteData == false).ToList();
        }
    }
}
