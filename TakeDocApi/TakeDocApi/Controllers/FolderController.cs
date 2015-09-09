using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TakeDocService.Folder.Interface;

namespace TakeDocApi.Controllers
{
    [RoutePrefix("folder")]
    public class FolderController : ApiController
    {
        [HttpGet]
        [Route("get/{userId}")]
        [TakeDocApi.Controllers.Security.AuthorizeTk()]
        public HttpResponseMessage Get(Guid userId)
        {
            try
            {
                IFolderService servFolder = Utility.MyUnityHelper.UnityHelper.Resolve<IFolderService>();
                ICollection<TakeDocModel.Folder> folders = servFolder.GetByPeriod(userId, System.DateTimeOffset.UtcNow.AddYears(-1), System.DateTimeOffset.UtcNow.AddYears(1));

                IList<object> items = new List<object>();

                foreach (TakeDocModel.Folder folder in folders)
                {
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
                    items.Add(json);
                }

                return Request.CreateResponse(items);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("set/{userId}")]
        [TakeDocApi.Controllers.Security.AuthorizeTk()]
        public HttpResponseMessage Set(Guid userId, [FromBody]string value)
        {
            try
            {
                Newtonsoft.Json.Linq.JObject data = Newtonsoft.Json.Linq.JObject.Parse(value);
                data.Add("userUpdateId",userId.ToString());
                IFolderService servFolder = Utility.MyUnityHelper.UnityHelper.Resolve<IFolderService>();
                servFolder.Update(data);

                return Request.CreateResponse("OK");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
