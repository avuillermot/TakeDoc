﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TakeDocService.Document.Interface;

namespace TakeDocApi.Controllers
{
    [RoutePrefix("Version")]
    public class VersionController : ApiController
    {
        [HttpGet]
        [Route("Binary/{versionId}/{entityId}")]
        public HttpResponseMessage GetBinaryFile(Guid versionId, Guid entityId)
        {
            try
            {
                IVersionService servVersion = Utility.MyUnityHelper.UnityHelper.Resolve<IVersionService>();
                byte[] data = servVersion.GetBinaryFile(versionId, entityId);

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
                IVersionService servVersion = Utility.MyUnityHelper.UnityHelper.Resolve<IVersionService>();
                string name = servVersion.GetUrlFile(versionId, entityId);

                return Request.CreateResponse(name);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
