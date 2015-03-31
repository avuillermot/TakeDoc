using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocService.Stats.Interface
{
    public interface IDashboardService
    {
        ICollection<TakeDocModel.Dto.Stats.StatusDocument> GetDashboard(Guid userId);
    }
}
