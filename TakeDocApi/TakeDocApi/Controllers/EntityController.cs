using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TakeDocApi.Controllers
{
    [RoutePrefix("Entity")]
    public class EntityController : ApiController
    {
        [HttpGet]
        [Route("UserId/{userId}")]
        public HttpResponseMessage GetEntityByUserId(Guid userId)
        {
            return Request.CreateResponse(HttpStatusCode.Accepted, "mm");
        }

        [HttpGet]
        [Route("UserReference/{userRef}")]
        public HttpResponseMessage GetEntityByUserReference(string userRef)
        {
            return Request.CreateResponse(HttpStatusCode.Accepted, "mm");
        }

        [HttpGet]
        [Route("All")]
        public HttpResponseMessage GetEntityByUserReference()
        {
            Utility.MyUnityHelper.UnityHelper.Init();
            TakeDocService.Document.Interface.ITypeDocumentService serv = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocService.Document.Interface.ITypeDocumentService>();
            ICollection<TakeDocModel.Type_Document> elems = serv.Get(System.Guid.NewGuid(),new System.Guid("55C72E33-8864-4E0E-9BC8-C82378B2BF8C"));
            var result = from e in elems select new { EntityId = e.EntityId, TypeDocumentLabel = e.TypeDocumentLabel };
            return Request.CreateResponse(HttpStatusCode.Accepted, result);
        }
    }
}
