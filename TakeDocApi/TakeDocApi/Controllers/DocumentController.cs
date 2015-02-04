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
        [Route("SetReceive/{documentId}")]
        public void SetReceive(Guid documentId)
        {
            IDocumentService servDocument = Utility.MyUnityHelper.UnityHelper.Resolve<IDocumentService>();
            servDocument.SetReceive(documentId);
        }
    }
}
