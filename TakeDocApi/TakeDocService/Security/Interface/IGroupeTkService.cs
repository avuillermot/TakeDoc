using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocService.Security.Interface
{
    public interface IGroupeTkService
    {
        ICollection<TakeDocModel.GroupTk> GetBy(Expression<Func<TakeDocModel.GroupTk, bool>> where, params Expression<Func<TakeDocModel.GroupTk, object>>[] properties);
    }
}
