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
        DAO.Interface.IDaoMetaData daoMetaData = UnityHelper.Resolve<DAO.Interface.IDaoMetaData>();
        DAO.Interface.IDaoDataFieldValue daoDataFieldValue = UnityHelper.Resolve<DAO.Interface.IDaoDataFieldValue>();
        DAO.Interface.IDaoDataFieldAutoComplete daoAutoComplete = UnityHelper.Resolve<DAO.Interface.IDaoDataFieldAutoComplete>();

        Interface.IDataFieldService servDataField = UnityHelper.Resolve<Interface.IDataFieldService>();
        Interface.IMetaDataFileService servMdFile = UnityHelper.Resolve<Interface.IMetaDataFileService>();

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

                daoMetaData.Add(meta);
            } 
        }

        public void SetMetaData(Guid userId, TakeDocModel.Document document, TakeDocModel.Entity entity, string json)
        {
            ICollection<TakeDocModel.MetaData> metadatas = daoMetaData.GetBy(x => x.MetaDataVersionId == document.DocumentCurrentVersionId && x.EtatDeleteData == false && x.EntityId == document.EntityId);
            ICollection<TakeDocModel.MetaDataFile> files = new List<TakeDocModel.MetaDataFile>();

            Newtonsoft.Json.Linq.JArray data = Newtonsoft.Json.Linq.JArray.Parse(json);
            foreach (Newtonsoft.Json.Linq.JObject obj in data)
            {
                string name = obj.Value<string>("name");

                TakeDocModel.MetaData meta = metadatas.First(x => x.MetaDataName == name);
                meta.MetaDataValue = obj.Value<string>("value");

                if (obj.Value<string>("type") != null && obj.Value<string>("type").ToUpper().Equals("FILE"))
                {
                    string fileName = obj.GetValue("file").Value<string>("name");
                    string filePath = obj.GetValue("file").Value<string>("path");
                    string dataFile = obj.GetValue("file").Value<string>("data");

                    // if no metadatavalue, it's to delete
                    bool toDel = string.IsNullOrEmpty(meta.MetaDataValue);
                    bool toUpdate = string.IsNullOrEmpty(fileName) == false && string.IsNullOrEmpty(dataFile) == true;
                    bool toAdd = string.IsNullOrEmpty(fileName) == false && string.IsNullOrEmpty(dataFile) == false;

                    if (toDel) servMdFile.Delete(meta.MetaDataId, userId);
                    else if (toAdd)
                    {
                        meta.MetaDataFile = new List<TakeDocModel.MetaDataFile>();
                        TakeDocModel.MetaDataFile file = new TakeDocModel.MetaDataFile();

                        file.EtatDeleteData = false;
                        file.MetaDataFilePath = filePath;
                        file.MetaDataId = meta.MetaDataId;
                        file.MetaDataFileName = servMdFile.GetFile(filePath).Name;
                        file.MetaDataFileData = Convert.FromBase64String(dataFile.Substring(dataFile.IndexOf(";base64,") + 8));

                        files.Add(file);
                    }
                }
            }

            ICollection<TakeDocModel.View_TypeDocumentDataField> fields = servDataField.GetDataField(document.DocumentTypeId, document.EntityId);
            foreach (TakeDocModel.MetaData meta in metadatas)
            {
                ICollection<TakeDocModel.View_TypeDocumentDataField> field = fields.Where(x => x.Reference == meta.MetaDataName && x.TypeDocumentId == document.DocumentTypeId).ToList();
                if (field.Count() > 0)
                {
                    bool ok = this.IsValid(field.First().TypeId, meta.MetaDataValue, field.First().Mandatory);
                    if (ok == false) throw new Exception("MetaData non valide.");
                }
            }
            try
            {
                daoMetaData.SetMetaData(userId, document.EntityId, document.DocumentTypeId, metadatas);
                foreach (TakeDocModel.MetaData metadata in metadatas)
                {
                    ICollection<TakeDocModel.MetaDataFile> metaFiles = files.Where(x => x.MetaDataId == metadata.MetaDataId).ToList();
                    if (metaFiles.Count() > 0)
                    {
                        foreach (TakeDocModel.MetaDataFile file in metaFiles)
                        {
                            if (file.MetaDataFileData != null) servMdFile.Update(file.MetaDataFilePath, file.MetaDataFileData, metadata.MetaDataId, userId, entity);
                        }
                    }
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex1)
            {
                base.Logger.Error(ex1);
                throw ex1;
            }
        }

        public bool IsValid(string typeName, string value, bool required)
        {
            bool result = true;
            try
            {
                this.Valid(typeName, value, required);
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        private void Valid(string typeName, string value, bool required)
        {
            if (string.IsNullOrEmpty(value) == true && required == true) throw new Exception("Bad data");
            if (typeName.StartsWith("System.String")) typeName = "System.String";
            Type myType = Type.GetType(typeName);
            bool ok = true;
            try
            {
                if (myType == typeof(System.String))
                {
                    if (value == null) value = string.Empty;
                    object o1 = Activator.CreateInstance(myType, new object[] { value.ToArray() });
                }
                else if (myType == typeof(System.Int32))
                {
                    int inInt = int.MinValue;
                    ok = int.TryParse(value, out inInt);
                    if (ok == false) throw new Exception(typeName);
                }
                else if (myType == typeof(System.DateTimeOffset))
                {
                   var ci = new CultureInfo("en-GB");
                   DateTimeOffset.Parse(value, ci.DateTimeFormat);
                }
                else if (myType == typeof(System.Boolean))
                {
                    Boolean.Parse(value);
                }
                else if (myType == typeof(System.Decimal))
                {
                    if (string.IsNullOrEmpty(value) == false) Decimal.Parse(value.Replace(".",","));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ICollection<TakeDocModel.MetaData> GetByVersion(Guid versionId, Guid entityId)
        {
            ICollection<TakeDocModel.MetaData> metas = daoMetaData.GetBy(x => x.MetaDataVersionId == versionId && x.EntityId == entityId && x.EtatDeleteData == false, x => x.DataField).ToList();
            foreach (TakeDocModel.MetaData meta in metas)
            {
                ICollection<TakeDocModel.DataFieldValue> values = daoDataFieldValue.GetBy(x => x.DataFieldId == meta.DataFieldId && x.EntityId == meta.EntityId && (x.EtatDeleteData == false || x.DataFieldValueKey == meta.MetaDataValue));
                if (values != null && values.Count() > 0) meta.DataFieldValues = values;
                else meta.DataFieldValues = new List<TakeDocModel.DataFieldValue>();
                
                ICollection<TakeDocModel.DataFieldAutoComplete> autoCompletes = daoAutoComplete.GetBy(x => x.DataFieldAutoCompleteId == meta.DataField.DataFieldAutoCompleteId && x.EntityId == meta.EntityId && x.EtatDeleteData == false);
                if (autoCompletes != null && autoCompletes.Count() > 0) meta.AutoComplete = autoCompletes.First();
                else meta.AutoComplete = null;

            }
            return metas;
        }

        public ICollection<TakeDocModel.Dto.Document.ReadOnlyMetadata> GetReadOnlyMetaData(TakeDocModel.Version version)
        {
            return this.GetReadOnlyMetaData(version.VersionId, version.EntityId);
        }

        private TakeDocModel.Dto.Document.ReadOnlyMetadata ToReadOnlyMetaData(TakeDocModel.MetaData metadata)
        {
            TakeDocModel.Dto.Document.ReadOnlyMetadata ro = new TakeDocModel.Dto.Document.ReadOnlyMetadata();
            ro.Name = metadata.MetaDataName;
            ro.EntityId = metadata.EntityId;
            ro.DisplayIndex = metadata.MetaDataDisplayIndex;
            ro.Label = metadata.DataField.DataFieldLabel;
            ro.Value = metadata.MetaDataValue;
            ro.Text = metadata.MetaDataValue;
            ro.Type = metadata.DataField.DataFieldType.DataFieldInputType;
            if (metadata.HtmlType.Equals("list") && string.IsNullOrEmpty(metadata.MetaDataValue) == false) ro.Text = metadata.DataFieldValues.First(x => x.DataFieldValueKey == metadata.MetaDataValue).DataFieldValueText;

            return ro;
        }

        public ICollection<TakeDocModel.Dto.Document.ReadOnlyMetadata> GetReadOnlyMetaData(Guid versionId, Guid entityId)
        {
            ICollection<TakeDocModel.Dto.Document.ReadOnlyMetadata> roMetas = new List<TakeDocModel.Dto.Document.ReadOnlyMetadata>();
            ICollection<TakeDocModel.MetaData> metadatas = this.GetByVersion(versionId, entityId);
            foreach (TakeDocModel.MetaData metadata in metadatas)
            {
                roMetas.Add(this.ToReadOnlyMetaData(metadata));
            }

            return roMetas;
        }

        public void Delete(Guid versionId, Guid entityId, Guid userId)
        {
            ICollection<TakeDocModel.MetaData> metas = daoMetaData.GetBy(x => x.MetaDataVersionId == versionId);
            foreach (TakeDocModel.MetaData meta in metas)
            {
                meta.EtatDeleteData = true;
                meta.DateDeleteData = System.DateTime.UtcNow;
                meta.UserDeleteData = userId;
                daoMetaData.Update(meta);
            }
        }
    }
}
