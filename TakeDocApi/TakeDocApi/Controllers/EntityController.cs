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
        [Route("user/add/{entityId}/{userId}")]
        public HttpResponseMessage AddUserToEntity(Guid userId, Guid entityId)
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

         [HttpPost]
        [Route("user/remove/{entityId}/{userId}")]
        public HttpResponseMessage RemoveUserToEntity(Guid userId, Guid entityId)
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
