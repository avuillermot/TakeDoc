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
        [Route("detailled/statusdocument/{userId}")]
        [TakeDocApi.Controllers.Security.AuthorizeTk()]
        public HttpResponseMessage GetDetailedDashboard(Guid userId)
        {
            TakeDocService.Stats.Interface.IDashboardService servDashboard = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocService.Stats.Interface.IDashboardService>();
            try
            {
                ICollection<TakeDocModel.Dto.Stats.StatsDocument> stats = servDashboard.GetDetailedDashboard(userId);
                return Request.CreateResponse(stats);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("simple/statusdocument/{userId}")]
        public HttpResponseMessage GetSimpleDashboard(Guid userId)
        {
            TakeDocService.Stats.Interface.IDashboardService servDashboard = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocService.Stats.Interface.IDashboardService>();
            try
            {
                ICollection<TakeDocModel.Dto.Stats.StatsDocument> stats = servDashboard.GetDashboard(userId);
                return Request.CreateResponse(stats);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
