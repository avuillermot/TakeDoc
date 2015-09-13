using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocDataAccess.Parameter.Interface
{
    public interface IDaoEntity
    {
        ICollection<TakeDocModel.Entity> GetBy(Expression<Func<TakeDocModel.Entity, bool>> where, params Expression<Func<TakeDocModel.Entity, object>>[] properties);
        void AddUser(TakeDocModel.UserTk user, TakeDocModel.Entity entity);
        void RemoveUser(TakeDocModel.UserTk user, TakeDocModel.Entity entity);
        ICollection<TakeDocModel.Entity> GetAll();
    }
}
