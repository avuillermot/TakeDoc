using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TakeDocApi.Controllers
{
    [RoutePrefix("dashboard")]
    public class DashboardController : ApiController
    {
        [HttpGet]
        [Route("statusdocument/{userId}")]
        public HttpResponseMessage GetDashboard(Guid userId)
        {
            TakeDocService.Stats.Interface.IDashboardService servDashboard = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocService.Stats.Interface.IDashboardService>();
            try
            {
                ICollection<TakeDocModel.Dto.Stats.StatusDocument> stats = servDashboard.GetDashboard(userId);
                return Request.CreateResponse(stats);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
