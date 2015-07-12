using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TakeDocApi.Controllers
{
    [RoutePrefix("workflow")]
    public class WorkflowController : ApiController
    {
        [HttpGet]
        [Route("history/{documentId}/{entityId}")]
        [TakeDocApi.Controllers.Security.AuthorizeTk()]
        public object GetStatusHistory(Guid documentId, Guid entityId)
        {
            TakeDocService.Workflow.Document.ValidationAuto servValidate = new TakeDocService.Workflow.Document.ValidationAuto();
            try
            {
                ICollection<object> histos = servValidate.GetStatusHistory(documentId, entityId);
                var req = new { value = histos };
                return Request.CreateResponse(req);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("answer/{workflowId}/{versionId}/{userId}/{answerId}")]
        [TakeDocApi.Controllers.Security.AuthorizeTk()]
        public object SetAnswer(Guid workflowId, Guid versionId, Guid userId, Guid answerId, [FromBody]string value)
        {
            TakeDocService.Workflow.Document.Answer servAnswer = new TakeDocService.Workflow.Document.Answer();
            try
            {
                Newtonsoft.Json.Linq.JObject data = Newtonsoft.Json.Linq.JObject.Parse(value);
                servAnswer.SetAnswer(workflowId, versionId, userId, answerId, data.Value<string>("comment"));
                return Request.CreateResponse();
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
