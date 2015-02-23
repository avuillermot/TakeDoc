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
        public void Post(Guid userId, Guid entityId, Guid versionId, string extension, [FromBody]string value)
        {
            IPageService servPage = Utility.MyUnityHelper.UnityHelper.Resolve<IPageService>();
            servPage.Add(userId, entityId, versionId, value, extension, 0);
        }
    }
}
