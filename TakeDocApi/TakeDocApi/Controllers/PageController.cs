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
        [TakeDocApi.Controllers.Security.AuthorizeTk()]
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

        [HttpGet]
        [Route("Image/{versionId}/{entityId}/{userId}")]
        [TakeDocApi.Controllers.Security.AuthorizeTk()]
        public HttpResponseMessage Post(Guid versionId, Guid entityId, Guid userId)
        {
            IVersionService servVersion = Utility.MyUnityHelper.UnityHelper.Resolve<IVersionService>();
            try
            {
                ICollection<object> back = servVersion.GetPages(versionId, entityId, userId);

                return Request.CreateResponse(back);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
