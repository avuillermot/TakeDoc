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

        public void SetMetaData(Guid userId, TakeDocModel.Document document, TakeDocModel.Entity entity, Newtonsoft.Json.Linq.JArray jsonMetaData)
        {
            ICollection<TakeDocModel.MetaData> metadatas = daoMetaData.GetBy(x => x.MetaDataVersionId == document.DocumentCurrentVersionId && x.EtatDeleteData == false && x.EntityId == document.EntityId);
            
            foreach (Newtonsoft.Json.Linq.JObject obj in jsonMetaData)
            {
                string name = obj.Value<string>("name");

                TakeDocModel.MetaData meta = metadatas.First(x => x.MetaDataName == name);
                meta.MetaDataValue = obj.Value<string>("value");
                meta.MetaDataText = obj.Value<string>("text");

                if (obj.Value<string>("type") != null && obj.Value<string>("type").ToUpper().Equals("SIGNATURE")) {
                    meta.MetaDataBlob = obj.Value<string>("value");
                    meta.MetaDataText = null;
                    meta.MetaDataValue = null;
                }
            }

            bool ok = this.BeProven(document, metadatas);
            if (ok == false) throw new Exception("MetaData non valide.");

            try
            {
                daoMetaData.SetMetaData(userId, document.EntityId, document.DocumentTypeId, metadatas);
             }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex1)
            {
                base.Logger.Error(ex1);
                throw ex1;
            }
        }

        /// <summary>
        /// Check if metadata is valid (type, mandatory, .....)
        /// </summary>
        /// <param name="document"></param>
        /// <param name="metadatas"></param>
        /// <returns></returns>
        public bool BeProven(TakeDocModel.Document document, ICollection<TakeDocModel.MetaData> metadatas)
        {
            ICollection<TakeDocModel.View_TypeDocumentDataField> fields = servDataField.GetDataField(document.DocumentTypeId, document.EntityId);
            foreach (TakeDocModel.MetaData meta in metadatas)
            {
                ICollection<TakeDocModel.View_TypeDocumentDataField> field = fields.Where(x => x.Reference == meta.MetaDataName && x.TypeDocumentId == document.DocumentTypeId).ToList();
                if (field.Count() > 0)
                {
                    string value = (string.IsNullOrEmpty(meta.MetaDataValue) == false) ? meta.MetaDataValue : meta.MetaDataBlob;
                    bool ok = this.BeProven(field.First().TypeId, value, field.First().Mandatory);
                    if (ok == false) throw new Exception(string.Format("Champ [{0}] non valide : {1}.", field.First().Label, meta.MetaDataValue));
                }
            }
            return true;
        }

        /// <summary>
        /// Check if value is valid
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="value"></param>
        /// <param name="required"></param>
        /// <returns></returns>
        public bool BeProven(string typeName, string value, bool required)
        {
            bool result = true;
            try
            {
                this.Assert(typeName, value, required);
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        private void Assert(string typeName, string value, bool required)
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
                    if (string.IsNullOrEmpty(value) == false)
                    {
                        var ci = new CultureInfo("en-GB");
                        DateTimeOffset.Parse(value, ci.DateTimeFormat);
                    }
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
                if (values != null && values.Count() > 0) meta.DataFieldValues = values.OrderBy(x => x.DataFieldValueKey).ToList();
                else meta.DataFieldValues = new List<TakeDocModel.DataFieldValue>();
                
                ICollection<TakeDocModel.DataFieldAutoComplete> autoCompletes = daoAutoComplete.GetBy(x => x.DataFieldAutoCompleteId == meta.DataField.DataFieldAutoCompleteId && x.EntityId == meta.EntityId && x.EtatDeleteData == false);
                if (autoCompletes != null && autoCompletes.Count() > 0) meta.AutoComplete = autoCompletes.First();
                else meta.AutoComplete = null;

            }
            return metas;
        }

        public ICollection<object> GetJson(ICollection<TakeDocModel.MetaData> metadatas)
        {
            ICollection<object> back = new List<object>();
            foreach (TakeDocModel.MetaData metadata in metadatas)
            {
                if (metadata.DataFieldValues.Count() > 0)
                {
                    var itemList = new
                    {
                        id = metadata.MetaDataId,
                        index = metadata.MetaDataDisplayIndex,
                        name = metadata.MetaDataName,
                        value = metadata.MetaDataValue,
                        text = metadata.MetaDataText,
                        mandatory = metadata.MetaDataMandatory,
                        label = metadata.DataField.DataFieldLabel,
                        type = metadata.DataField.DataFieldType.DataFieldInputType,
                        htmlType = metadata.HtmlType,
                        entityId = metadata.EntityId,
                        valueList = from value in metadata.DataFieldValues
                                    select new
                                    {
                                        id = value.DataFieldId,
                                        index = value.DataFieldValueIndex,
                                        key = value.DataFieldValueKey,
                                        text = value.DataFieldValueText,
                                        reference = value.DataFieldValueReference,
                                        etatDelete = value.EtatDeleteData,
                                        entity = value.EntityId
                                    }
                    };
                    back.Add(itemList);
                }
                else if (metadata.AutoComplete != null)
                {
                    var itemAutoComplete = new
                    {
                        id = metadata.MetaDataId,
                        index = metadata.MetaDataDisplayIndex,
                        name = metadata.MetaDataName,
                        value = metadata.MetaDataValue,
                        text = metadata.MetaDataText,
                        mandatory = metadata.MetaDataMandatory,
                        label = metadata.DataField.DataFieldLabel,
                        type = metadata.DataField.DataFieldType.DataFieldInputType,
                        htmlType = metadata.HtmlType,
                        entityId = metadata.EntityId,
                        autoCompleteId = (metadata.AutoComplete == null) ? Guid.Empty : metadata.AutoComplete.DataFieldAutoCompleteId,
                        autoCompleteTitle = (metadata.AutoComplete == null) ? null : metadata.AutoComplete.DataFieldAutoCompleteTitle,
                        autoCompletePlaceHolder = (metadata.AutoComplete == null) ? null : metadata.AutoComplete.DataFieldAutoCompletePlaceHolder,
                        autoCompleteUrl = (metadata.AutoComplete == null) ? null : metadata.AutoComplete.DataFieldAutoCompleteUrl,
                        autoCompleteReference = (metadata.AutoComplete == null) ? null : metadata.AutoComplete.DataFieldAutoCompleteReference
                    };
                    back.Add(itemAutoComplete);
                }
                else
                {
                    string inputType = metadata.DataField.DataFieldType.DataFieldTypeId;
                    object value = null;
                    if (inputType.ToUpper().Equals("SYSTEM.BOOLEAN") && string.IsNullOrEmpty(metadata.MetaDataValue) == true)
                        value = false;

                    if (string.IsNullOrEmpty(metadata.MetaDataValue) == false)
                    {
                        value = metadata.MetaDataValue;
                        if (inputType.ToUpper().Equals("SYSTEM.BOOLEAN"))
                        {
                            if (value.ToString().ToUpper().Equals("FALSE")) value = false;
                            else value = true;
                        }
                        else if (metadata.DataField.DataFieldTypeId.ToUpper().Equals("SIGNATURE"))
                            value = metadata.MetaDataBlob;
                    }
                    var itemSimple = new
                    {
                        id = metadata.MetaDataId,
                        index = metadata.MetaDataDisplayIndex,
                        name = metadata.MetaDataName,
                        value = value,
                        mandatory = metadata.MetaDataMandatory,
                        label = metadata.DataField.DataFieldLabel,
                        type = metadata.DataField.DataFieldType.DataFieldInputType,
                        htmlType = metadata.HtmlType,
                        entityId = metadata.EntityId
                    };

                    

                    back.Add(itemSimple);
                }
            }
            return back;
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
            ro.Text = metadata.MetaDataText;
            ro.Type = metadata.DataField.DataFieldType.DataFieldInputType;

            if (metadata.DataField.DataFieldTypeId.ToUpper().Equals("SIGNATURE"))
                ro.Value = metadata.MetaDataBlob;
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
