using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TakeDocApi.Controllers
{
    [RoutePrefix("tovalidate")]
    public class DocumentToValidateController : ApiController
    {
        [HttpGet]
        [Route("manager/{userId}")]
        public HttpResponseMessage GetByManager(Guid userId)
        {
            TakeDocService.Workflow.Document.Interface.IDocumentToValidate servToValidate = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocService.Workflow.Document.Interface.IDocumentToValidate>();
            try
            {
                ICollection<TakeDocModel.DocumentToValidate> docs = servToValidate.GetByManager(userId);
                var req = new {value = docs};
                return Request.CreateResponse(req);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("typedocument/{userId}")]
        public HttpResponseMessage GetByTypeDocument(Guid userId)
        {
            TakeDocService.Workflow.Document.Interface.IDocumentToValidate servToValidate = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocService.Workflow.Document.Interface.IDocumentToValidate>();
            try
            {
                ICollection<TakeDocModel.DocumentToValidate> docs = servToValidate.GetByTypeDocument(userId);
                return Request.CreateResponse(docs);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
