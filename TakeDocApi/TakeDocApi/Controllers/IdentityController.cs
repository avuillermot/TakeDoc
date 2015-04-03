using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Security.Claims;
using TakeDocService.Security.Interface;
using Utility.MyUnityHelper;
using System.Web;
using System.Threading;
using System.Security.Principal;

namespace TakeDocApi.Controllers
{
    [RoutePrefix("identity")]
    public class IdentityController : ApiController
    {
        [HttpPost]
        [Route("logon")]
        [AllowAnonymous]
        public HttpResponseMessage Post([FromBody]string value)
        {
            IUserTkService servUser = UnityHelper.Resolve<IUserTkService>();
            Newtonsoft.Json.Linq.JObject data = Newtonsoft.Json.Linq.JObject.Parse(value);
            try
            {
                TakeDocModel.UserTk user = servUser.Logon(data.Value<string>("login"), data.Value<string>("password"));

                ClaimsPrincipal cp = servUser.GetClaimsPrincipal(user);

                SetPrincipal(cp);
                return Request.CreateResponse(HttpStatusCode.OK, user);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("all")]
        public HttpResponseMessage GetAll()
        {
            TakeDocService.Security.Interface.IUserTkService servUser = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocService.Security.Interface.IUserTkService>();
            try
            {
                ICollection<TakeDocModel.UserTk> users = servUser.GetAll();
                return Request.CreateResponse(users);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        private void SetPrincipal(IPrincipal principal)
        {
            Thread.CurrentPrincipal = principal;
            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = principal;
            }
        }

        [HttpGet]
        [Route("activate/{userReference}")]
        public HttpResponseMessage Activate(string userReference)
        {
            TakeDocService.Security.Interface.IUserTkService servUser = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocService.Security.Interface.IUserTkService>();
            try
            {
                bool back = servUser.ActivateUser(userReference);
                return Request.CreateResponse(back);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("request/account")]
        public HttpResponseMessage RequestAccount([FromBody]string value)
        {
            TakeDocService.Workflow.Security.Interface.IAccount servAccount = UnityHelper.Resolve<TakeDocService.Workflow.Security.Interface.IAccount>();
            Newtonsoft.Json.Linq.JObject data = Newtonsoft.Json.Linq.JObject.Parse(value);

            try
            {
                string entityRef = data.Value<string>("entity");
                string firstName = data.Value<string>("firstName");
                string lastName = data.Value<string>("lastName");
                string email = data.Value<string>("email");
                string password1 = data.Value<string>("password1");
                string password2 = data.Value<string>("password2");
                string culture = data.Value<string>("culture");

                bool ok = servAccount.CreateRequest(firstName, lastName, email, password1, culture, entityRef);

                return Request.CreateResponse(HttpStatusCode.OK, ok);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


    }
}
