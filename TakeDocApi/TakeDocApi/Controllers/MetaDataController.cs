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
                var req = from metadata in metadatas
                          select new
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
                                          },
                              autoCompleteId = (metadata.AutoComplete == null) ? Guid.Empty : metadata.AutoComplete.DataFieldAutoCompleteId,
                              autoCompleteTitle = (metadata.AutoComplete == null) ? null : metadata.AutoComplete.DataFieldAutoCompleteTitle,
                              autoCompletePlaceHolder = (metadata.AutoComplete == null) ? null : metadata.AutoComplete.DataFieldAutoCompletePlaceHolder,
                              autoCompleteUrl = (metadata.AutoComplete == null) ? null : metadata.AutoComplete.DataFieldAutoCompleteUrl,
                              autoCompleteReference = (metadata.AutoComplete == null) ? null : metadata.AutoComplete.DataFieldAutoCompleteReference
                          };

                return Request.CreateResponse(req.ToList<object>());
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("Version/{versionId}/{userId}/{entityId}")]
        [TakeDocApi.Controllers.Security.AuthorizeTk()]
        public HttpResponseMessage SetMetaData(Guid versionId, Guid userId, Guid entityId, [FromBody]string value)
        {
            IDocumentService servDocument = Utility.MyUnityHelper.UnityHelper.Resolve<IDocumentService>();
            try
            {
                Newtonsoft.Json.Linq.JArray data = Newtonsoft.Json.Linq.JArray.Parse(value);
                IDictionary<string, string> metadatas = new Dictionary<string, string>();
                foreach (Newtonsoft.Json.Linq.JObject obj in data)
                {
                    string name = obj.Value<string>("name");
                    string input = obj.Value<string>("type").ToUpper();

                    metadatas.Add(name, obj.Value<string>("value"));
                }
                servDocument.SetMetaData(userId, entityId, versionId, metadatas);
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
