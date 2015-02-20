using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace TakeDocDataAccess.Document.Interface
{
    public interface IDaoTypeDocument
    {
        ICollection<TakeDocModel.Type_Document> GetBy(Expression<Func<TakeDocModel.Type_Document, bool>> where, params Expression<Func<TakeDocModel.Type_Document, object>>[] properties);
        //ICollection<TakeDocModel.Type_Document> Get(Guid userId, Guid entityId);
    }
}
