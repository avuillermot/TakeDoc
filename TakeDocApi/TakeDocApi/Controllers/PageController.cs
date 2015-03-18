using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Drawing;
using System.IO;
using TakeDocService.Document.Interface;

namespace TakeDocApi.Controllers
{
    [RoutePrefix("Page")]
    public class PageController : ApiController
    {
        [HttpPost]
        [Route("Add")]
        public HttpResponseMessage Post(Guid userId, Guid entityId, Guid versionId, string extension, int rotation, [FromBody]string value)
        {
            IPageService servPage = Utility.MyUnityHelper.UnityHelper.Resolve<IPageService>();
            try
            {
                servPage.Add(userId, entityId, versionId, value, extension, rotation);
                return Request.CreateResponse(true);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
