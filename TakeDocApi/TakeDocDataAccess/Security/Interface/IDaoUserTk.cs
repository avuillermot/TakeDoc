using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocDataAccess.Security.Interface
{
    public interface IDaoUserTk
    {
        ICollection<TakeDocModel.UserTk> GetBy(Expression<Func<TakeDocModel.UserTk, bool>> where, params Expression<Func<TakeDocModel.UserTk, object>>[] properties);
        ICollection<TakeDocModel.UserTk> GetAll();
        TakeDocModel.UserTk Create(TakeDocModel.UserTk user);
        void Delete(TakeDocModel.UserTk user);
        void Delete(ICollection<TakeDocModel.UserTk> items);
        void AddEntity(TakeDocModel.UserTk user, TakeDocModel.Entity entity);
        void Update(TakeDocModel.UserTk user);
    }
}
