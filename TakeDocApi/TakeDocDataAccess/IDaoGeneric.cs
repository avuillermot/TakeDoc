using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocDataAccess
{
    public interface IDaoGeneric<T>
    {
        ICollection<T> GetAll();
        ICollection<T> GetBy(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] properties);
        void Update(T item);
    }
}
