using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocDataAccess.Document.Interface
{
    public interface IDaoMetaData
    {
        TakeDocModel.MetaData Add(TakeDocModel.MetaData meta);
        void SetMetaData(Guid userId, Guid entityId, Guid versionId, ICollection<TakeDocModel.MetaData> metadatas);
        ICollection<TakeDocModel.MetaData> GetBy(Expression<Func<TakeDocModel.MetaData, bool>> where, params Expression<Func<TakeDocModel.MetaData, object>>[] properties);
        void Update(TakeDocModel.MetaData version);
    }
}
