using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace TakeDocApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            TakeDocModel.Environnement.Init(System.Configuration.ConfigurationManager.AppSettings);
        }

        /*protected void Application_BeginRequest()
        {
            string s = "";
        }

        protected void Application_EndRequest()
        {
            foreach (TakeDocModel.TakeDocEntities1 value in HttpContext.Current.Items.Values.OfType<TakeDocModel.TakeDocEntities1>())
            {
                value.Dispose();
            }
            Context.HttpDbContext ctx = (Context.HttpDbContext)HttpContext.Current.Items[TakeDocApi.Context.HttpDbContext.DbContextKey];
            ctx.RemoveValue();
        }*/
    }
}
