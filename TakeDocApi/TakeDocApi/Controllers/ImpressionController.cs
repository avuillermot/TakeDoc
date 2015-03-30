using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TakeDocService.Impression.Interface;

namespace TakeDocApi.Controllers
{
    [RoutePrefix("Impression")]
    public class ImpressionController : ApiController
    {
        [HttpGet]
        [Route("Binary/{versionId}/{entityId}")]
        public HttpResponseMessage GetBinaryFile(Guid versionId, Guid entityId)
        {
            try
            {
                IReportVersionService servReport = Utility.MyUnityHelper.UnityHelper.Resolve<IReportVersionService>();
                byte[] data = servReport.GetBinaryFile(versionId, entityId);

                return Request.CreateResponse(data);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("Url/{versionId}/{entityId}")]
        public HttpResponseMessage GetUrlFile(Guid versionId, Guid entityId)
        {
            try
            {
                IReportVersionService servReport = Utility.MyUnityHelper.UnityHelper.Resolve<IReportVersionService>();
                string name = servReport.GetUrlFile(versionId, entityId);

                return Request.CreateResponse(name);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
