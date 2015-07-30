using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TakeDocService.Document.Interface;

namespace TakeDocApi.Controllers
{
    [RoutePrefix("MetaData")]
    public class MetaDataController : ApiController
    {
        [HttpGet]
        [Route("Version/ReadOnly/{versionId}/{entityId}")]
        [TakeDocApi.Controllers.Security.AuthorizeTk()]
        public HttpResponseMessage GetReadOnlyMetaData(Guid versionId, Guid entityId)
        {
            try
            {
                IMetaDataService servMetaData = Utility.MyUnityHelper.UnityHelper.Resolve<IMetaDataService>();
                ICollection<TakeDocModel.Dto.Document.ReadOnlyMetadata> metas = servMetaData.GetReadOnlyMetaData(versionId, entityId);

                return Request.CreateResponse(metas);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("Version/{versionId}/{entityId}")]
        [TakeDocApi.Controllers.Security.AuthorizeTk()]
        public HttpResponseMessage GetMetaData(Guid versionId, Guid entityId)
        {
            IMetaDataService servMetaData = Utility.MyUnityHelper.UnityHelper.Resolve<IMetaDataService>();

            try
            {
                ICollection<TakeDocModel.MetaData> metadatas = servMetaData.GetByVersion(versionId, entityId);
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
                            mandatory = metadata.MetaDataMandatory,
                            type = metadata.DataField.DataFieldType.DataFieldInputType,
                            label = metadata.DataField.DataFieldLabel,
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
                            mandatory = metadata.MetaDataMandatory,
                            type = metadata.DataField.DataFieldType.DataFieldInputType,
                            label = metadata.DataField.DataFieldLabel,
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
                    else if (metadata.DataField.DataFieldTypeId.ToUpper().Equals("FILE")) {
                        TakeDocModel.MetaDataFile file = metadata.MetaDataFile.First();
                        var itemFile = new
                        {
                            id = metadata.MetaDataId,
                            index = metadata.MetaDataDisplayIndex,
                            name = metadata.MetaDataName,
                            value = metadata.MetaDataValue,
                            mandatory = metadata.MetaDataMandatory,
                            type = metadata.DataField.DataFieldType.DataFieldInputType,
                            label = metadata.DataField.DataFieldLabel,
                            htmlType = metadata.HtmlType,
                            entityId = metadata.EntityId,
                            file = new
                            {
                                id = file.MetaDataFileId,
                                reference = file.MetaDataFileReference,
                                data = file.MetaDataFileData,
                                path = file.MetaDataFilePath,
                                name = file.MetaDataFileName,
                                mimeType = file.MetaDataFileMimeType,
                                extension = file.MetaDataFileExtension
                            }
                        };
                        back.Add(itemFile);
                    }
                    else
                    {
                        var itemSimple = new
                        {
                            id = metadata.MetaDataId,
                            index = metadata.MetaDataDisplayIndex,
                            name = metadata.MetaDataName,
                            value = metadata.MetaDataValue,
                            mandatory = metadata.MetaDataMandatory,
                            type = metadata.DataField.DataFieldType.DataFieldInputType,
                            label = metadata.DataField.DataFieldLabel,
                            htmlType = metadata.HtmlType,
                            entityId = metadata.EntityId
                        };
                        back.Add(itemSimple);
                    }
                }

                return Request.CreateResponse(back);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("Version/{versionId}/{userId}/{entityId}")]
        [TakeDocApi.Controllers.Security.AuthorizeTk()]
        public HttpResponseMessage SetMetaData(Guid versionId, Guid userId, Guid entityId, [FromBody]string value)
        {
            IDocumentService servDocument = Utility.MyUnityHelper.UnityHelper.Resolve<IDocumentService>();
            try
            {
                servDocument.SetMetaData(userId, entityId, versionId, value);
                return Request.CreateResponse();
            }
            catch (Exception ex)
            {
                var msg = new {Message = ex.Message};
                return Request.CreateResponse(HttpStatusCode.InternalServerError, msg);
            }
        }
    }
}
