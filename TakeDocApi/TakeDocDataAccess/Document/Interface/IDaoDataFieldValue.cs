using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocDataAccess.Document.Interface
{
    public interface IDaoDataFieldValue
    {
        ICollection<TakeDocModel.DataFieldValue> GetBy(Expression<Func<TakeDocModel.DataFieldValue, bool>> where, params Expression<Func<TakeDocModel.DataFieldValue, object>>[] properties);
    }
}
