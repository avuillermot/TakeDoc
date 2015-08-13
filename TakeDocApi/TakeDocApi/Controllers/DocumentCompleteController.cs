using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TakeDocService.Document.Interface;
using TakeDocApi.Controllers.Security;

namespace TakeDocApi.Controllers
{
    [RoutePrefix("DocumentComplete")]
    public class DocumentCompleteController : ApiController
    {
        [HttpGet]
        [Route("version/{versionId}/{userId}/{entityId}")]
        [TakeDocApi.Controllers.Security.AuthorizeTk()]
        public HttpResponseMessage GetByVersion(Guid versionId, Guid userId, Guid entityId)
        {
            TakeDocService.Document.Interface.IDocumentCompleteService servDocTk = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocService.Document.Interface.IDocumentCompleteService>();
            TakeDocService.Document.Interface.IMetaDataService servMetadata = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocService.Document.Interface.IMetaDataService>();
            try
            {
                TakeDocModel.Dto.Document.DocumentComplete myDoc = servDocTk.GetByVersion(versionId, userId, entityId);
                ICollection<object> json = servMetadata.GetJson(myDoc.MetaDatas);
                var back = new
                {
                    MetaDatas = json,
                    Document = myDoc.Document,
                    Pages = myDoc.Pages
                };

                return Request.CreateResponse(back);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("{userId}/{entityId}/{startWorkflow}")]
        [TakeDocApi.Controllers.Security.AuthorizeTk()]
        public HttpResponseMessage Post(Guid userId, Guid entityId, bool startWorkflow, [FromBody]string value)
        {
            TakeDocService.Document.Interface.IDocumentCompleteService servDocTk = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocService.Document.Interface.IDocumentCompleteService>();
            try
            {
                servDocTk.Update(userId, entityId, value, startWorkflow);
                return Request.CreateResponse();
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
