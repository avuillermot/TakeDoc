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
    }
}
