using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TakeDocService.Print.Interface;

namespace TakeDocApi.Controllers
{
    [RoutePrefix("Print")]
    public class PrintController : ApiController
    {
        [HttpGet]
        [Route("Binary/{versionId}/{entityId}")]
        [TakeDocApi.Controllers.Security.AuthorizeTk()]
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
        [Route("Url/Document/{versionId}/{entityId}")]
        [TakeDocApi.Controllers.Security.AuthorizeTk()]
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

        [HttpGet]
        [Route("Url/File/{metadataId}/{entityId}")]
        [TakeDocApi.Controllers.Security.AuthorizeTk()]
        public HttpResponseMessage GetUrlMetaDataFile(Guid metadataId, Guid entityId)
        {
            try
            {
                TakeDocService.Document.Interface.IMetaDataFileService servFile = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocService.Document.Interface.IMetaDataFileService>();
                string name = servFile.GetUrlFile(metadataId, entityId);
                return Request.CreateResponse(name);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [HttpPost]
        [Route("generatepdf/version/{versionId}/{entityId}/{userId}")]
        public HttpResponseMessage GeneratePdf(Guid versionId, Guid entityId, Guid userId)
        {
            try
            {
                IReportVersionService serv = Utility.MyUnityHelper.UnityHelper.Resolve<IReportVersionService>();
                serv.Generate(versionId, entityId);
                return Request.CreateResponse(true);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
