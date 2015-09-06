using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace TakeDocDataAccess.Folder.Interface
{
    public interface IDaoFolder
    {
        TakeDocModel.Folder Create(TakeDocModel.Folder toCreate, Guid userCreateId, Guid entityId);
        void Delete(Guid folderId, Guid userId, Guid entityId);
        TakeDocModel.Folder Update(TakeDocModel.Folder toUpdate, Guid userCreateId, Guid entityId);
        ICollection<TakeDocModel.Folder> GetBy(Expression<Func<TakeDocModel.Folder, bool>> where, params Expression<Func<TakeDocModel.Folder, object>>[] properties);
        
    }
}
