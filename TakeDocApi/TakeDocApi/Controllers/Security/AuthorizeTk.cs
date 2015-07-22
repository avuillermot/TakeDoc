using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Web.Http;

namespace TakeDocApi.Controllers.Security
{
    public static class Roles
    {
        public const string Administrator = "ADMIN";
        public const string Backoffice = "BACKOFFICE";
        public const string User = "USER";
    }
    [System.AttributeUsage(System.AttributeTargets.Method)]
    public class AuthorizeTk : AuthorizeAttribute
    {
        TakeDocService.Security.Interface.ITokenService servToken = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocService.Security.Interface.ITokenService>();

        public AuthorizeTk(params string[] roles)
        {
            base.Roles = string.Join(",",roles);
        }

        protected override bool IsAuthorized(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            return true;
            if (actionContext.Request.Headers.Authorization == null) return false;
            Guid accessTokenId = new Guid(actionContext.Request.Headers.Authorization.Scheme);
            bool back = servToken.IsValidAccessToken(accessTokenId, Roles.Split(','));
            return back;
        }
    }
}
