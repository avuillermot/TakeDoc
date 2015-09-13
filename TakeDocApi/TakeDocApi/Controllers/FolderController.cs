using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TakeDocService.Folder.Interface;
using Newtonsoft.Json.Linq;

namespace TakeDocApi.Controllers
{
    [RoutePrefix("folder")]
    public class FolderController : ApiController
    {
        [HttpPost]
        [Route("get/{userId}")]
        [TakeDocApi.Controllers.Security.AuthorizeTk()]
        public HttpResponseMessage Get(Guid userId, [FromBody]string value)
        {
            try
            {
                Newtonsoft.Json.Linq.JObject data = Newtonsoft.Json.Linq.JObject.Parse(value);
                DateTimeOffset start = DateTimeOffset.Parse(data.Value<string>("start"), System.Globalization.CultureInfo.InvariantCulture);
                DateTimeOffset end = DateTimeOffset.Parse(data.Value<string>("end"), System.Globalization.CultureInfo.InvariantCulture);

                string agendas = data.Value<string>("agendas");
                ICollection<Guid> myAgendas = new List<Guid>();
                foreach (JValue agenda in Newtonsoft.Json.Linq.JArray.Parse(agendas))
                {
                    myAgendas.Add(new Guid(agenda.Value<string>()));
                }

                IFolderService servFolder = Utility.MyUnityHelper.UnityHelper.Resolve<IFolderService>();
                ICollection<object> folders = servFolder.GetJsonByPeriod(myAgendas, start, end);

                return Request.CreateResponse(folders);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("put/{userId}")]
        [TakeDocApi.Controllers.Security.AuthorizeTk()]
        public HttpResponseMessage Set(Guid userId, [FromBody]string value)
        {
            try
            {
                Newtonsoft.Json.Linq.JObject data = Newtonsoft.Json.Linq.JObject.Parse(value);
                data.Add("userUpdateId",userId.ToString());
                IFolderService servFolder = Utility.MyUnityHelper.UnityHelper.Resolve<IFolderService>();
                TakeDocModel.Folder folder = servFolder.Create(data);

                var json = new
                {
                    id = folder.FolderId,
                    folderId = folder.FolderId,
                    title = folder.FolderLabel,
                    start = folder.FolderDateStart,
                    end = folder.FolderDateEnd,
                    allDay = false,
                    entityId = folder.EntityId,
                    ownerId = folder.FolderOwnerId,
                    folderTypeId = folder.FolderTypeId,
                };

                return Request.CreateResponse(json);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("set/{userId}")]
        [TakeDocApi.Controllers.Security.AuthorizeTk()]
        public HttpResponseMessage Put(Guid userId, [FromBody]string value)
        {
            try
            {
                Newtonsoft.Json.Linq.JObject data = Newtonsoft.Json.Linq.JObject.Parse(value);
                data.Add("userUpdateId", userId.ToString());
                IFolderService servFolder = Utility.MyUnityHelper.UnityHelper.Resolve<IFolderService>();
                servFolder.Update(data);

                return Request.CreateResponse("OK");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [Route("delete/{folderId}/{userId}/{entityId}")]
        [TakeDocApi.Controllers.Security.AuthorizeTk()]
        public HttpResponseMessage Put(Guid folderId, Guid userId, Guid entityId)
        {
            try
            {
                IFolderService servFolder = Utility.MyUnityHelper.UnityHelper.Resolve<IFolderService>();
                servFolder.Delete(folderId, userId, entityId);

                return Request.CreateResponse("OK");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
