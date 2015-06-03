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
        [Route("answer/{workflowId}/{userId}/approve")]
        public object SetAnswer(Guid workflowId, Guid userId)
        {
            TakeDocService.Workflow.Document.ValidationAuto servValidate = new TakeDocService.Workflow.Document.ValidationAuto();
            try
            {
                /*ICollection<object> histos = servValidate.GetStatusHistory(documentId, entityId);
                var req = new { value = histos };*/
                return Request.CreateResponse();
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
