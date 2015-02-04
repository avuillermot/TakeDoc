using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocDataAccess.Document.Interface
{
    public interface IDaoPageStoreLocator
    {
        ICollection<TakeDocModel.View_PageStoreLocator> GetBy(Expression<Func<TakeDocModel.View_PageStoreLocator, bool>> where, params Expression<Func<TakeDocModel.View_PageStoreLocator, object>>[] properties);
    }
}
