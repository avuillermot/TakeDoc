using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocDataAccess.Document.Interface
{
    public interface IDaoBackOfficeTypeDocument
    {
        void Add(Guid userIdToAdd, Guid typeDocumentId, Guid entityId, Guid userIdUpdater);
        void Delete(Guid userIdToDel, Guid typeDocumentId, Guid entityId, Guid userIdUpdater);
        ICollection<TakeDocModel.BackOfficeTypeDocument> GetBy(Expression<Func<TakeDocModel.BackOfficeTypeDocument, bool>> where, params Expression<Func<TakeDocModel.BackOfficeTypeDocument, object>>[] properties);
    }
}
