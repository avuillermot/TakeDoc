using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TakeDocApi.Controllers
{
    [RoutePrefix("TypeDocument")]
    public class TypeDocumentController : ApiController
    {
        [HttpPost]
        [Route("Update")]
        public HttpResponseMessage Update([FromBody]string value)
        {
            TakeDocService.Document.Interface.ITypeDocumentService servTypeDocument = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocService.Document.Interface.ITypeDocumentService>();
            try
            {
                Newtonsoft.Json.Linq.JArray data = Newtonsoft.Json.Linq.JArray.Parse(value);
                foreach (Newtonsoft.Json.Linq.JObject obj in data)
                {
                    string typeDocumentRef = "";
                    string dataFieldRef = "";
                    bool mandatory = true;
                    int index = 1;
                    string entityRef = "";
                    string userRef = "";
                    /*this.set("id", id);
        this.set("reference", reference);
        this.set("label", label);
        this.set("index", index);
        this.set("inputType", inputType);
        this.set("inputTypeLabel", this.getInputTypeLabel(inputType));
        this.set("isList", false);
        this.set("isAutocomplete", false);
        this.set("autoCompleteId", false);
        this.set("mandatory", false);
        this.set("delete", false);
        this.set("entityId", entityId);*/


                    servTypeDocument.AddDataField(typeDocumentRef, dataFieldRef, mandatory, index, entityRef, userRef);
                }

                
                return Request.CreateResponse();
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
