using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TakeDocApi.Controllers.AutoComplete
{
    [RoutePrefix("UserTk")]
    public class UserTkController : ApiController
    {
        [HttpGet]
        [Route("ByName/{currentUserId}/{value}/{entityId}")]
        public HttpResponseMessage SetSend(Guid currentUserId, string value, Guid entityId)
        {
            TakeDocService.Search.Interface.ISearch search = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocService.Search.Interface.ISearch>();
            try
            {
                ICollection<TakeDocModel.SearchUserTkFullText_Result> users = search.SearchUserFullText(currentUserId, value, entityId, true);
                var req = from user in users
                          select new
                          {
                              key = user.UserTkId,
                              text = user.UserTkFirstName + " " + user.UserTkLastName
                          };
                return Request.CreateResponse(req.ToList());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        [Route("ByName/{currentUserId}/{value}")]
        public HttpResponseMessage SetSend(Guid currentUserId, string value)
        {
            TakeDocService.Search.Interface.ISearch search = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocService.Search.Interface.ISearch>();
            try
            {
                ICollection<TakeDocModel.SearchUserTkFullText_Result> users = search.SearchUserFullText(currentUserId, value, Guid.Empty, true);
                var req = from user in users
                          select new
                          {
                              key = user.UserTkId,
                              text = user.UserTkFirstName + " " + user.UserTkLastName
                          };
                return Request.CreateResponse(req.ToList());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }   }
}
