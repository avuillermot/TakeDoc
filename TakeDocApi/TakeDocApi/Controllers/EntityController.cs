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
                TakeDocService.Parameter.Interface.IEntityService servEntity = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocService.Parameter.Interface.IEntityService>();
                servEntity.AddUser(userId, entityId);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [Route("user/remove/{entityId}/{userId}")]
        public HttpResponseMessage RemoveUserToEntity(Guid userId, Guid entityId)
        {
            try
            {
                TakeDocService.Parameter.Interface.IEntityService servEntity = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocService.Parameter.Interface.IEntityService>();
                servEntity.RemoveUser(userId, entityId);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
