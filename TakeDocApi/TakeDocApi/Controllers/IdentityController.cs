using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Security.Claims;
using TakeDocService.Security.Interface;
using Utility.MyUnityHelper;

namespace TakeDocApi.Controllers
{
    [RoutePrefix("identity")]
    public class IdentityController : ApiController
    {
        [HttpPost]
        [Route("logon")]
        public TakeDocModel.UserTk Post([FromBody]string value)
        {
            IUserTkService servUser = UnityHelper.Resolve<IUserTkService>();
            Newtonsoft.Json.Linq.JObject data = Newtonsoft.Json.Linq.JObject.Parse(value);
            return servUser.Logon(data.Value<string>("login"), data.Value<string>("password"));
        }
    }
}
