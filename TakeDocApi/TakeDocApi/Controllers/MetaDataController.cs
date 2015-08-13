using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TakeDocService.Document.Interface;

namespace TakeDocApi.Controllers
{
    [RoutePrefix("MetaData")]
    public class MetaDataController : ApiController
    {
        [HttpGet]
        [Route("Version/ReadOnly/{versionId}/{entityId}")]
        [TakeDocApi.Controllers.Security.AuthorizeTk()]
        public HttpResponseMessage GetReadOnlyMetaData(Guid versionId, Guid entityId)
        {
            try
            {
                IMetaDataService servMetaData = Utility.MyUnityHelper.UnityHelper.Resolve<IMetaDataService>();
                ICollection<TakeDocModel.Dto.Document.ReadOnlyMetadata> metas = servMetaData.GetReadOnlyMetaData(versionId, entityId);

                return Request.CreateResponse(metas);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("Version/{versionId}/{entityId}")]
        [TakeDocApi.Controllers.Security.AuthorizeTk()]
        public HttpResponseMessage GetMetaData(Guid versionId, Guid entityId)
        {
            IMetaDataService servMetaData = Utility.MyUnityHelper.UnityHelper.Resolve<IMetaDataService>();

            try
            {
                ICollection<TakeDocModel.MetaData> metadatas = servMetaData.GetByVersion(versionId, entityId);
                ICollection<object> json = servMetaData.GetJson(metadatas);
                return Request.CreateResponse(json);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
