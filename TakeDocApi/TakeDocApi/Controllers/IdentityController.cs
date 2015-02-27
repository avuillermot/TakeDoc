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
        [HttpGet]
        [Route("logon/{login}/{password}")]
        public TakeDocModel.UserTk Logon(string login, string password) {
            IUserTkService servUser = UnityHelper.Resolve<IUserTkService>();
            return servUser.Logon(login, password);
        }
    }
}
