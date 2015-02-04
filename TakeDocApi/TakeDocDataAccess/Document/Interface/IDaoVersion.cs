using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace TakeDocDataAccess.Document.Interface
{
    public interface IDaoVersion
    {
        TakeDocModel.Version Create(Guid userId, Guid entityId, Guid versionId, Guid documentId, decimal versionNumber);
        TakeDocModel.Version GetLastVersion(Guid documentId);
        ICollection<TakeDocModel.Version> GetBy(Expression<Func<TakeDocModel.Version, bool>> where, params Expression<Func<TakeDocModel.Version, object>>[] properties);
        void Update(TakeDocModel.Version version);
    }
}
