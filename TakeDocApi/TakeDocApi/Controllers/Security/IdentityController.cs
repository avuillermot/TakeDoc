using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Security.Claims;
using Thinktecture.IdentityModel.Client;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace TakeDocApi.Controllers.Security
{
    [RoutePrefix("identity")]
    /*[Authorize]*/
    public class IdentityController : ApiController
    {
        [HttpGet]
        [Route("get")]
        public IHttpActionResult Get()
        {
            var user = User as ClaimsPrincipal;
            var claims = from c in user.Claims
                         select new
                         {
                             type = c.Type,
                             value = c.Value
                         };

            return Json(claims);
        }

        private async Task<TokenResponse> GetTokenAsync()
        {
            var client = new OAuth2Client(
                new Uri("https://localhost/identity/connect/token"),
                "1",
                "test_by_avt");

            return await client.RequestClientCredentialsAsync("sampleApi");
        }

        private async Task<string> CallApi(string token)
        {
            var client = new HttpClient();
            client.SetBearerToken(token);

            var json = await client.GetStringAsync("https://localhost/identity");
            return JArray.Parse(json).ToString();
        }

        [HttpGet]
        [Route("test")]
        public async Task<string> ClientCredentials()
        {
            var response = await GetTokenAsync();
            var result = await CallApi(response.AccessToken);

            return result;
        }
    }
}
