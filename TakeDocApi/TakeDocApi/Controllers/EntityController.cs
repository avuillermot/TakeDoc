using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TakeDocApi.Controllers
{
    [RoutePrefix("entity")]
    public class EntityController : ApiController
    {
        [HttpPost]
        [Route("add/user/{userId}/{entityId}")]
        public HttpResponseMessage AddEntityToUser(Guid userId, Guid entityId)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [Route("delete/user/{userId}/{entityId}")]
        public HttpResponseMessage RemoveEntityToUser(Guid userId, Guid entityId)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
