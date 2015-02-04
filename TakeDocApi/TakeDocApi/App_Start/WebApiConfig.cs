using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.OData.Builder;
using TakeDocModel;

namespace TakeDocApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Configuration et services API Web

            // Itinéraires de l'API Web
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<TakeDocModel.Document>("Documents");
            builder.EntitySet<TakeDocModel.Statut_Document>("oStatut_Document");
            builder.EntitySet<TakeDocModel.Statut_Version>("oStatut_Version");
            builder.EntitySet<TakeDocModel.Version>("oVersion");
            builder.EntitySet<TakeDocModel.Page>("oPage");
            builder.EntitySet<TakeDocModel.Type_Document>("oType_Document");
            builder.EntitySet<TakeDocModel.MetaData>("oMetaData");
            config.Routes.MapODataRoute("odata", "odata", builder.GetEdmModel());
        }
    }
}
