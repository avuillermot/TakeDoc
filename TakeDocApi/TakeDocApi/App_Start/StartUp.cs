using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace TakeDocApi.App_Start
{
    //[assembly: OwinStartup(typeof(TakeDocApi.App_Start.StartUp))]
    public class StartUp
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "ApplicationCookie",
                //LoginPath = new PathString("/auth/login") url de la page d'authentification
            });
        }
    }
}