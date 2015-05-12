using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TakeDocApi.Controllers
{
    [RoutePrefix("TypeDocument")]
    public class TypeDocumentController : ApiController
    {
        [HttpPut]
        [Route("Add/{label}/{userId}/{entityId}")]
        public HttpResponseMessage Update(string label, Guid userId, Guid entityId)
        {
            TakeDocService.Document.Interface.ITypeDocumentService servTypeDocument = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocService.Document.Interface.ITypeDocumentService>();
            try
            {
                TakeDocModel.TypeDocument back = servTypeDocument.Add(label, entityId, userId);
                return Request.CreateResponse(back.TypeDocumentId);
            }
            catch (Exception ex)
            {
                TakeDocService.LoggerService.CreateError(ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [Route("Delete/{typeDocumentId}/{userId}/{entityId}")]
        public HttpResponseMessage Delete(Guid typeDocumentId, Guid userId, Guid entityId)
        {
            TakeDocService.Document.Interface.ITypeDocumentService servTypeDocument = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocService.Document.Interface.ITypeDocumentService>();
            try
            {
                servTypeDocument.Delete(typeDocumentId, userId, entityId);
                return Request.CreateResponse();
            }
            catch (Exception ex)
            {
                TakeDocService.LoggerService.CreateError(ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("Update/{userId}")]
        public HttpResponseMessage Update(Guid userId, [FromBody]string value)
        {
            TakeDocService.Document.Interface.ITypeDocumentService servTypeDocument = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocService.Document.Interface.ITypeDocumentService>();
            try
            {
                Newtonsoft.Json.Linq.JObject data = Newtonsoft.Json.Linq.JObject.Parse(value);
                Guid typeDocumentId = new Guid(data.Value<string>("id"));
                Guid entityId =  new Guid(data.Value<string>("entityId"));
                TakeDocModel.TypeDocument type = servTypeDocument.GetBy(x => x.TypeDocumentId == typeDocumentId && x.EntityId == entityId).First();
                type.TypeDocumentLabel = data.Value<string>("label");
                type.TypeDocumentPageNeed = data.Value<bool>("pageNeed");
                bool deleted = data.Value<bool>("deleted");
                if (type.EtatDeleteData != deleted)
                {
                    type.EtatDeleteData = deleted;
                    type.UserDeleteData = userId;
                    type.DateDeleteData = System.DateTimeOffset.UtcNow;
                }
                type.TypeDocumentValidationId =  new Guid(data.Value<string>("typeValidationId"));

                servTypeDocument.Update(type, userId);

                return Request.CreateResponse();
            }
            catch (Exception ex)
            {
                TakeDocService.LoggerService.CreateError(ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("Update/DataField/{typeDocumentId}/{userId}/{entityId}")]
        public HttpResponseMessage Update(Guid typeDocumentId, Guid userId, Guid entityId, [FromBody]string value)
        {
            TakeDocService.Document.Interface.ITypeDocumentService servTypeDocument = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocService.Document.Interface.ITypeDocumentService>();
            try
            {
                Newtonsoft.Json.Linq.JArray data = Newtonsoft.Json.Linq.JArray.Parse(value);
                foreach (Newtonsoft.Json.Linq.JObject obj in data)
                {
                    string dataFieldRef = obj.Value<string>("reference");
                    bool mandatory = obj.Value<bool>("mandatory");
                    int index = obj.Value<int>("index");
                    bool delete = obj.Value<bool>("deleted");
                    servTypeDocument.AddDataField(typeDocumentId, dataFieldRef, mandatory, delete, index, entityId, userId);
                }
                return Request.CreateResponse();
            }
            catch (Exception ex)
            {
                TakeDocService.LoggerService.CreateError(ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("add/backofficeuser/{typeDocumentId}/{backOfficerUserId}/{entityId}/{userIdUpdater}")]
        public HttpResponseMessage AddBackOfficeUser(Guid typeDocumentId, Guid backOfficerUserId, Guid entityId, Guid userIdUpdater)
        {
            TakeDocService.Document.Interface.ITypeDocumentService servTypeDocument = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocService.Document.Interface.ITypeDocumentService>();
            try
            {
                servTypeDocument.AddBackOfficeUser(backOfficerUserId, typeDocumentId, entityId, userIdUpdater);
                return Request.CreateResponse(true);
            }
            catch (Exception ex)
            {
                TakeDocService.LoggerService.CreateError(ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [Route("delete/backofficeuser/{typeDocumentId}/{backOfficerUserId}/{entityId}/{userIdUpdater}")]
        public HttpResponseMessage DeleteBackOfficeUser(Guid typeDocumentId, Guid backOfficerUserId, Guid entityId, Guid userIdUpdater)
        {
            TakeDocService.Document.Interface.ITypeDocumentService servTypeDocument = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocService.Document.Interface.ITypeDocumentService>();
            try
            {
                servTypeDocument.DeleteBackOfficeUser(backOfficerUserId, typeDocumentId, entityId, userIdUpdater);
                return Request.CreateResponse(true);
            }
            catch (Exception ex)
            {
                TakeDocService.LoggerService.CreateError(ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("get/backofficeuser/{typeDocumentId}/{entityId}")]
        public HttpResponseMessage DeleteBackOfficeUser(Guid typeDocumentId, Guid entityId)
        {
            TakeDocService.Document.Interface.ITypeDocumentService servTypeDocument = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocService.Document.Interface.ITypeDocumentService>();
            try
            {
                ICollection<TakeDocModel.UserTk> users = servTypeDocument.GetBackOfficeUser(typeDocumentId, entityId);
                var req = from user in users
                          select new
                          {
                              id = user.UserTkId,
                              fullName = string.Concat(user.UserTkFirstName," ", user.UserTkLastName),
                              deleted = false,
                              entityId = entityId
                          };
                return Request.CreateResponse(req);
            }
            catch (Exception ex)
            {
                TakeDocService.LoggerService.CreateError(ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
