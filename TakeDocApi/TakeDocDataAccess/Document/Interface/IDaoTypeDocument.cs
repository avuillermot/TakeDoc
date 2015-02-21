using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace TakeDocDataAccess.Document.Interface
{
    public interface IDaoTypeDocument
    {
        ICollection<TakeDocModel.TypeDocument> GetBy(Expression<Func<TakeDocModel.TypeDocument, bool>> where, params Expression<Func<TakeDocModel.TypeDocument, object>>[] properties);
        //ICollection<TakeDocModel.Type_Document> Get(Guid userId, Guid entityId);
    }
}
