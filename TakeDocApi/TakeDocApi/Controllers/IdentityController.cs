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
using TakeDocApi.Controllers.Security;

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
                return Request.CreateResponse(HttpStatusCode.OK, user);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("user/{userId}")]
        [TakeDocApi.Controllers.Security.AuthorizeTk()]
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
        [TakeDocApi.Controllers.Security.AuthorizeTk()]
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
        [TakeDocApi.Controllers.Security.AuthorizeTk(Roles.Backoffice, Roles.Administrator)]
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

        [HttpGet]
        [Route("activate/{userReference}")]
        [TakeDocApi.Controllers.Security.AuthorizeTk(Roles.Backoffice, Roles.Administrator)]
        public HttpResponseMessage ActivateUser(string userReference)
        {
            TakeDocService.Workflow.Security.Interface.IAccount servAccount = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocService.Workflow.Security.Interface.IAccount>();
            try
            {
                bool back = servAccount.ActivateUser(userReference);
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
        [Route("update/{userId}/{firstName}/{lastName}/{email}/{culture}/{enable}/{activate}/{groupId}/{managerId}")]
        [TakeDocApi.Controllers.Security.AuthorizeTk()]
        public HttpResponseMessage UpdateUserTk(Guid userId, string firstName, string lastName, string email, string culture, bool enable, bool activate, Guid groupId, Guid? managerId) {
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
                user.UserTkEnable = enable;
                user.UserTkActivate = activate;
                user.UserTkGroupId = groupId;
                user.UserTkManagerId = managerId;
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
        [TakeDocApi.Controllers.Security.AuthorizeTk()]
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

        [HttpPost]
        [Route("generate/password/{userId}")]
        [TakeDocApi.Controllers.Security.AuthorizeTk()]
        public HttpResponseMessage GenerateNewPassword(Guid userId)
        {
            try
            {
                TakeDocService.Security.Interface.IUserTkService servUser = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocService.Security.Interface.IUserTkService>();
                servUser.GenerateNewPassword(userId);
                return Request.CreateResponse(HttpStatusCode.OK, true);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [Route("delete/{userId}/{currentUserId}")]
        [TakeDocApi.Controllers.Security.AuthorizeTk(Roles.Backoffice, Roles.Administrator)]
        public HttpResponseMessage Delete(Guid userId, Guid currentUserId)
        {
            try
            {
                TakeDocService.Security.Interface.IUserTkService servUser = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocService.Security.Interface.IUserTkService>();
                servUser.Delete(userId, currentUserId);
                return Request.CreateResponse(HttpStatusCode.OK, true);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("name/{userId}")]
        [TakeDocApi.Controllers.Security.AuthorizeTk()]
        public HttpResponseMessage GetById(Guid userId)
        {
            try
            {
                TakeDocService.Security.Interface.IUserTkService servUser = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocService.Security.Interface.IUserTkService>();
                TakeDocModel.UserTk user = servUser.GetBy(x => x.UserTkId == userId).First();
                return Request.CreateResponse(HttpStatusCode.OK, string.Concat(user.UserTkFirstName, " ", user.UserTkLastName));
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
