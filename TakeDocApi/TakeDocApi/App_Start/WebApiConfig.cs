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
            builder.EntitySet<TakeDocModel.Status_Document>("Status_Documents");
            builder.EntitySet<TakeDocModel.Status_Version>("Status_Versions");
            builder.EntitySet<TakeDocModel.Version>("Versions");
            builder.EntitySet<TakeDocModel.Page>("Pages");
            builder.EntitySet<TakeDocModel.TypeDocument>("TypeDocuments");
            builder.EntitySet<TakeDocModel.MetaData>("MetaDatas");
            builder.EntitySet<TakeDocModel.DataField>("DataFields");
            builder.EntitySet<TakeDocModel.DataFieldType>("DataFieldTypes");
            builder.EntitySet<TakeDocModel.DataFieldValue>("DataFieldValues");
            builder.EntitySet<TakeDocModel.DataFieldAutoComplete>("AutoCompletes");
            builder.EntitySet<TakeDocModel.View_DocumentExtended>("DocumentExtendeds");
            builder.EntitySet<TakeDocModel.GroupTk>("GroupTks");
            builder.EntitySet<TakeDocModel.WorkflowType>("WorkflowTypes");
            builder.EntitySet<TakeDocModel.View_TypeDocumentDataField>("TypeDocumentDataFields");
            builder.EntitySet<TakeDocModel.BackOfficeTypeDocument>("BackOfficeTypeDocuments");
            builder.EntitySet<TakeDocModel.DocumentStatusHisto>("DocumentStatusHistos");
            builder.EntitySet<TakeDocModel.Workflow>("Workflows");
            builder.EntitySet<TakeDocModel.WorkflowType>("WorkflowTypes");
            builder.EntitySet<TakeDocModel.WorkflowAnswer>("WorkflowAnswers");
               
            config.Routes.MapODataRoute("odata", "odata", builder.GetEdmModel());
        }
    }
}
