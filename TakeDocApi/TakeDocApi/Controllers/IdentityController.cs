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
        public HttpResponseMessage Logon([FromBody]string value)
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
        [Route("user/{userId}")]
        [AllowAnonymous]
        public HttpResponseMessage GetUser(Guid userId)
        {
            IUserTkService servUser = UnityHelper.Resolve<IUserTkService>();
            try
            {
                ICollection<TakeDocModel.UserTk> users = servUser.GetBy(x => x.UserTkId == userId);
                TakeDocModel.UserTk user = users.First();

                return Request.CreateResponse(HttpStatusCode.OK, user);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPatch]
        [Route("ChangePassword")]
        [AllowAnonymous]
        public HttpResponseMessage ChangeUserPassword([FromBody]string value)
        {
            IUserTkService servUser = UnityHelper.Resolve<IUserTkService>();
            Newtonsoft.Json.Linq.JObject data = Newtonsoft.Json.Linq.JObject.Parse(value);
            try
            {
                servUser.ChangePassword(new Guid(data.Value<string>("userId")),data.Value<string>("olderPassword"),data.Value<string>("newPassword"));

                return Request.CreateResponse(HttpStatusCode.OK, true);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("all")]
        public HttpResponseMessage GetAllUser()
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
        public HttpResponseMessage ActivateUser(string userReference)
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

        [HttpPatch]
        [Route("update/{userId}/{firstName}/{lastName}/{email}/{culture}")]
        public HttpResponseMessage UpdateUserTk(Guid userId, string firstName, string lastName, string email, string culture) {
            try
            {
                TakeDocService.Security.Interface.IUserTkService servUser = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocService.Security.Interface.IUserTkService>();
                ICollection<TakeDocModel.UserTk> users = servUser.GetBy(x => x.UserTkId == userId);
                if (users.Count != 1) throw new Exception("Invalid user");
                TakeDocModel.UserTk user = users.First();
                user.UserTkFirstName = firstName;
                user.UserTkLastName = lastName;
                user.UserTkEmail = email;
                user.UserTkCulture = culture;
                servUser.Update(user);
                return Request.CreateResponse(HttpStatusCode.OK, true);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("user/entity/{userId}")]
        public HttpResponseMessage GetEntityUser(Guid userId)
        {
            try
            {
                TakeDocService.Security.Interface.IView_UserEntityService servUser = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocService.Security.Interface.IView_UserEntityService>();
                ICollection<TakeDocModel.View_UserEntity> back = servUser.GetByUser(userId);
                return Request.CreateResponse(HttpStatusCode.OK, back);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


    }
}
