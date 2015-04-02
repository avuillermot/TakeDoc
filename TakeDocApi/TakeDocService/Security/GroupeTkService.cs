using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocService.Security
{
    public class GroupeTkService : BaseService, Interface.IGroupeTkService
    {
        TakeDocDataAccess.DaoBase<TakeDocModel.GroupTk> daoGroup = new TakeDocDataAccess.DaoBase<TakeDocModel.GroupTk>();

        public ICollection<TakeDocModel.GroupTk> GetBy(Expression<Func<TakeDocModel.GroupTk, bool>> where, params Expression<Func<TakeDocModel.GroupTk, object>>[] properties)
        {
            return daoGroup.GetBy(where, properties).ToList(); ;
        }

    }
}
