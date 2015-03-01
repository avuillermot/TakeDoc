using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Security.Claims;
using TakeDocService.Security.Interface;
using Utility.MyUnityHelper;
using System.Web.Http.Owin;
using System.Web;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace TakeDocApi.Controllers
{
    [RoutePrefix("identity")]
    public class IdentityController : ApiController
    {
        [HttpPost]
        [Route("logon")]
        [AllowAnonymous]
        public async Task<TakeDocModel.UserTk> Post([FromBody]string value)
        {
            IUserTkService servUser = UnityHelper.Resolve<IUserTkService>();
            Newtonsoft.Json.Linq.JObject data = Newtonsoft.Json.Linq.JObject.Parse(value);
            TakeDocModel.UserTk user = servUser.Logon(data.Value<string>("login"), data.Value<string>("password"));
            ClaimsPrincipal cp = servUser.GetClaimsPrincipal(user);

            await SignInAsync(cp, isPersistent: false);
            //Request.GetOwinContext().Authentication.SignInn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
            //Request.GetRequestContext().Principal = cp;
            //System.Threading.Thread.CurrentPrincipal = cp;
            //authManager.SignOut("ApplicationCookie");
            //HttpContext.Current.User = cp;
            //User.Identity

            return user;
        }

        private async Task SignInAsync(ClaimsPrincipal cp, bool isPersistent)
        {
            Request.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

            /*var identity = await UserManager.CreateIdentityAsync(
               user, DefaultAuthenticationTypes.ApplicationCookie);*/

            Request.GetOwinContext().Authentication.SignIn(
               new AuthenticationProperties()
               {
                   IsPersistent = isPersistent
               }, cp.Identities.First());
        }
    }
}
