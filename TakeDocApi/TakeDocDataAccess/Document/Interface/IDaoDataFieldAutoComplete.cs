using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocDataAccess.Document.Interface
{
    public interface IDaoDataFieldAutoComplete
    {
        ICollection<TakeDocModel.DataFieldAutoComplete> GetBy(Expression<Func<TakeDocModel.DataFieldAutoComplete, bool>> where, params Expression<Func<TakeDocModel.DataFieldAutoComplete, object>>[] properties);
    }
}
