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
            builder.EntitySet<TakeDocModel.Statut_Document>("Statut_Documents");
            builder.EntitySet<TakeDocModel.Statut_Version>("Statut_Versions");
            builder.EntitySet<TakeDocModel.Version>("Versions");
            builder.EntitySet<TakeDocModel.Page>("Pages");
            builder.EntitySet<TakeDocModel.TypeDocument>("TypeDocuments");
            builder.EntitySet<TakeDocModel.MetaData>("MetaDatas");
            builder.EntitySet<TakeDocModel.DataField>("DataFields");
            config.Routes.MapODataRoute("odata", "odata", builder.GetEdmModel());
        }
    }
}
