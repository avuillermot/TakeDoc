using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocDataAccess.Document.Interface
{
    public interface IDaoVersionStoreLocator
    {
        ICollection<TakeDocModel.View_VersionStoreLocator> GetBy(Expression<Func<TakeDocModel.View_VersionStoreLocator, bool>> where, params Expression<Func<TakeDocModel.View_VersionStoreLocator, object>>[] properties);
    }
}
