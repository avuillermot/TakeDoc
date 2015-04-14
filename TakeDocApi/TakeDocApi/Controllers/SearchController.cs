using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TakeDocApi.Controllers
{
    [RoutePrefix("search")]
    public class SearchController : ApiController
    {
        [HttpPost]
        [Route("user")]
        public HttpResponseMessage SearchUser([FromBody]string value)
        {
            TakeDocService.Search.Interface.ISearch search = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocService.Search.Interface.ISearch>();
            try
            {
                Newtonsoft.Json.Linq.JObject data = Newtonsoft.Json.Linq.JObject.Parse(value);
                string firstName = data.Value<string>("firstName");
                string lastName = data.Value<string>("lastName");
                string email = data.Value<string>("email");
                string strEntityId = data.Value<string>("entityId");

                Guid entityId = string.IsNullOrEmpty(strEntityId) ? Guid.Empty : new Guid(strEntityId);

                return Request.CreateResponse(search.SearchUser(firstName, lastName, email, entityId));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

        }
    }
}
