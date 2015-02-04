using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocDataAccess.Document.Interface
{
    public interface IDaoDocument
    {
        TakeDocModel.Document Create( Guid userId, Guid entityId, Guid documentId, Guid versionId, Guid typeDocumentId, string documentLabel);
        ICollection<TakeDocModel.Document> GetBy(Expression<Func<TakeDocModel.Document, bool>> where, params Expression<Func<TakeDocModel.Document, object>>[] properties);
        void Update(TakeDocModel.Document document);
    }
}
