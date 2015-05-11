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
        [Route("ByName/{userId}/{value}")]
        public HttpResponseMessage SetSend(Guid userId, string value)
        {
            //TakeDocService.Security.Interface.IUserTkService servUser = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocService.Security.Interface.IUserTkService>();
            TakeDocService.Search.Interface.ISearch search = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocService.Search.Interface.ISearch>();
            try
            {
                //ICollection<TakeDocModel.UserTk> users = servUser.GetByName(userId, value);
                ICollection<TakeDocModel.SearchUserTkFullText_Result> users = search.SearchUserFullText(userId, value);
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
    }
}
