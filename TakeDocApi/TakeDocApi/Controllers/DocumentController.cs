using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TakeDocService.Document.Interface;
using TakeDocApi.Controllers.Security;

namespace TakeDocApi.Controllers
{
    [RoutePrefix("Document")]
    public class DocumentController : ApiController
    {
        [HttpGet]
        [Route("SetIncomplete/{documentId}/{userId}")]
        [TakeDocApi.Controllers.Security.AuthorizeTk()]
        public HttpResponseMessage SetIncomplete(Guid documentId, Guid userId)
        {
            TakeDocService.Workflow.Document.Interface.IStatus servStatus = new TakeDocService.Workflow.Document.Status();
            try
            {
                servStatus.SetStatus(documentId, TakeDocModel.Status_Document.Incomplete, userId, true);
                return Request.CreateResponse();
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("SetArchive/{documentId}/{userId}")]
        [TakeDocApi.Controllers.Security.AuthorizeTk()]
        public HttpResponseMessage SetArchive(Guid documentId, Guid userId)
        {
            TakeDocService.Workflow.Document.Interface.IStatus servStatus = new TakeDocService.Workflow.Document.Status();
            try
            {
                servStatus.SetStatus(documentId, TakeDocModel.Status_Document.Archive, userId, true);
                return Request.CreateResponse();
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [Route("delete/{documentId}/{entityId}/{userId}")]
        [TakeDocApi.Controllers.Security.AuthorizeTk()]
        public HttpResponseMessage DeleteDocument(Guid documentId, Guid entityId, Guid userId)
        {
            IDocumentService servDocument = Utility.MyUnityHelper.UnityHelper.Resolve<IDocumentService>();
            try
            {
                servDocument.Delete(documentId, entityId, userId);
                return Request.CreateResponse();
            }
            catch (Exception ex)
            {
                Utility.Logger.myLogger.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("search/{title}/{typeDocumentId}/{entityId}/{userId}")]
        [TakeDocApi.Controllers.Security.AuthorizeTk()]
        public HttpResponseMessage SearchDocument(string title, Guid typeDocumentId, Guid entityId, Guid userId, [FromBody]string values)
        {
            IDocumentService servDocument = Utility.MyUnityHelper.UnityHelper.Resolve<IDocumentService>();
            try
            {
                ICollection<TakeDocModel.Dto.Document.SearchMetadata> searchs = new List<TakeDocModel.Dto.Document.SearchMetadata>();

                if (values != null)
                {
                    Newtonsoft.Json.Linq.JArray data = Newtonsoft.Json.Linq.JArray.Parse(values);
                    foreach (Newtonsoft.Json.Linq.JObject obj in data)
                    {
                        string name = obj.Value<string>("name");
                        string condition = obj.Value<string>("condition");
                        string value = obj.Value<string>("value");
                        string ctype = obj.Value<string>("type");

                        TakeDocModel.DataField field = new TakeDocModel.DataField()
                        {
                            DataFieldType = new TakeDocModel.DataFieldType()
                            {
                                DataFieldInputType = ctype
                            }
                        };

                        searchs.Add(new TakeDocModel.Dto.Document.SearchMetadata(condition)
                        {
                            MetaDataValue = value,
                            MetaDataName = name,
                            Condition = condition,
                            DataField = field
                        });
                    }
                }
               
                ICollection<TakeDocModel.View_DocumentExtended> back = servDocument.Search(title, typeDocumentId, searchs, userId, entityId);
                return Request.CreateResponse(new {value = back});
            }
            catch (Exception ex)
            {
                Utility.Logger.myLogger.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
