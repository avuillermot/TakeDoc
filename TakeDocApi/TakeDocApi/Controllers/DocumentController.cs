using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TakeDocService.Document.Interface;

namespace TakeDocApi.Controllers
{
    [RoutePrefix("Document")]
    public class DocumentController : ApiController
    {
        [HttpGet]
        [Route("SetDataSend/{documentId}")]
        public HttpResponseMessage SetSend(Guid documentId)
        {
            IDocumentService servDocument = Utility.MyUnityHelper.UnityHelper.Resolve<IDocumentService>();
            try
            {
                servDocument.SetStatus(documentId, TakeDocModel.Status_Document.DataSend);
                return Request.CreateResponse();
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
