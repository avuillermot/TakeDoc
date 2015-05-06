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
                    bool delete = obj.Value<bool>("delete");
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
    }
}
