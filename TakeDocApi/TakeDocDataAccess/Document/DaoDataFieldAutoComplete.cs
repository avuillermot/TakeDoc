using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocDataAccess.Document
{
    public class DaoDataFieldAutoComplete : DaoBase<TakeDocModel.DataFieldAutoComplete>, Interface.IDaoDataFieldAutoComplete
    {
        public ICollection<TakeDocModel.DataFieldAutoComplete> GetBy(Expression<Func<TakeDocModel.DataFieldAutoComplete, bool>> where, params Expression<Func<TakeDocModel.DataFieldAutoComplete, object>>[] properties)
        {
            return base.GetBy(where, properties);
        }
    }
}
