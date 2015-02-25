using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Thinktecture.IdentityServer.Core.Configuration;
using Thinktecture.IdentityServer.Core.Services.InMemory;
using Owin;
using Microsoft.Owin;
using Thinktecture.IdentityServer.AccessTokenValidation;

[assembly: OwinStartup(typeof(TakeDocApi.Controllers.oAuth2.Startup))]
namespace TakeDocApi.Controllers.oAuth2
{
    public class Startup
    {
        /*public void Configuration(IAppBuilder app)
        {
            var factory = InMemoryFactory.Create(
                scopes: Scopes.Get(),
                clients: Clients.Get(),
                users: Users.Get().ToList()
            );

            IdentityServerOptions options = new IdentityServerOptions() { Factory = factory };
            app.Map("/identity", idsrvApp => idsrvApp.UseIdentityServer(options));
        }*/

        public void Configuration(IAppBuilder app)
        {
            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                Authority = "https://localhost:44319/identity",
                RequiredScopes = new[] { "sampleApi" }
            });

            // web api configuration
            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();

            app.UseWebApi(config);
        }
    }
}