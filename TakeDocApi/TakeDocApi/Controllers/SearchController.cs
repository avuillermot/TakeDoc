using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TakeDocApi.Controllers.Security;

namespace TakeDocApi.Controllers
{
    [RoutePrefix("search")]
    public class SearchController : ApiController
    {
        [HttpPost]
        [Route("user")]
        [TakeDocApi.Controllers.Security.AuthorizeTk(Roles.Backoffice, Roles.Administrator)]
        public HttpResponseMessage SearchUser([FromBody]string value)
        {
            TakeDocService.Search.Interface.ISearch search = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocService.Search.Interface.ISearch>();
            try
            {
                Newtonsoft.Json.Linq.JObject data = Newtonsoft.Json.Linq.JObject.Parse(value);
                string strSearchUserId = data.Value<string>("userId");
                string firstName = data.Value<string>("firstName");
                string lastName = data.Value<string>("lastName");
                string email = data.Value<string>("email");
                string strEntityId = data.Value<string>("entityId");

                Guid entityId = string.IsNullOrEmpty(strEntityId) ? Guid.Empty : new Guid(strEntityId);
                Guid userId = string.IsNullOrEmpty(strSearchUserId) ? Guid.Empty : new Guid(strSearchUserId);

                return Request.CreateResponse(search.SearchUser(userId, firstName, lastName, email, entityId));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
