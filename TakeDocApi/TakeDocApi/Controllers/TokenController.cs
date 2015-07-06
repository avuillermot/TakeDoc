using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TakeDocApi.Controllers
{
    [RoutePrefix("token")]
    public class TokenController : ApiController
    {
        [HttpGet]
        [Route("create/{userId}/{source}/{clientId}")]
        public HttpResponseMessage CreateRefreshToken(Guid userId, string source, string clientId)
        {
            TakeDocService.Security.Interface.ITokenService token = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocService.Security.Interface.ITokenService>();
            try
            {
                TakeDocModel.RefreshToken refresh = token.CreateRefreshToken(userId, source);

                var back = new {
                    RefreshToken = refresh.Id,
                    AccessToken = refresh.AccessToken.First().Id,
                    AccessTokenEndDate = refresh.AccessToken.First().DateEndUTC
                };

                return Request.CreateResponse(back);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("access/{refreshTokenId}")]
        public HttpResponseMessage CreateAccessToken(Guid refreshTokenId)
        {
            TakeDocService.Security.Interface.ITokenService token = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocService.Security.Interface.ITokenService>();
            try
            {
                TakeDocModel.AccessToken access = token.GetAccessToken(refreshTokenId);

                var back = new
                {
                    AccessToken = access.Id,
                    AccessTokenEndDate = access.DateEndUTC
                };

                return Request.CreateResponse(back);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
