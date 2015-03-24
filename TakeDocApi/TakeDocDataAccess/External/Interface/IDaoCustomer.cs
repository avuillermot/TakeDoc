using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocDataAccess.External.Interface
{
    public interface IDaoCustomer
    {
        ICollection<TakeDocModel.Customer> GetBy(Expression<Func<TakeDocModel.Customer, bool>> where, params Expression<Func<TakeDocModel.Customer, object>>[] properties);
    }
}
