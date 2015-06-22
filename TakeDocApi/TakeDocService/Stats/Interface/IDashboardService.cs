using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocService.Stats.Interface
{
    public interface IDashboardService
    {
        /// <summary>
        /// Dashboard by entity, by type document type, by status
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        ICollection<TakeDocModel.Dto.Stats.StatsDocument> GetDetailedDashboard(Guid userId);
        /// <summary>
        /// Dashboard by status
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        ICollection<TakeDocModel.Dto.Stats.StatsDocument> GetDashboard(Guid userId);
    }
}
