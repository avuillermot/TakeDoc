using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocDataAccess.Document.Interface
{
    public interface IDaoPage
    {
        TakeDocModel.Page Add(Guid userId, Guid entityId, Guid versionId, int rotation);
        void Update(TakeDocModel.Page item);
        ICollection<TakeDocModel.Page> GetBy(Expression<Func<TakeDocModel.Page, bool>> where, params Expression<Func<TakeDocModel.Page, object>>[] properties);
    }
}
