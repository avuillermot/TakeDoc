using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace TakeDocDataAccess.Document.Interface
{
    public interface IDaoView_DocumentExtended
    {
        ICollection<TakeDocModel.View_DocumentExtended> Search(string title, string reference, Guid typeDocumentId, ICollection<TakeDocModel.Dto.Document.SearchMetadata> metadatas, Guid userId, Guid entityId);
        ICollection<TakeDocModel.View_DocumentExtended> GetBy(Expression<Func<TakeDocModel.View_DocumentExtended, bool>> where, params Expression<Func<TakeDocModel.View_DocumentExtended, object>>[] properties);
    }
}
