using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace TakeDocDataAccess.Folder
{
    public class DaoFolder : DaoBase<TakeDocModel.Folder>, Interface.IDaoFolder
    {
        public TakeDocModel.Folder Create(TakeDocModel.Folder toCreate, Guid userCreateId, Guid entityId)
        {
            toCreate.FolderId = System.Guid.NewGuid();
            toCreate.FolderReference = base.Context.GenerateReference("Folder");
            toCreate.DateCreateData = System.DateTimeOffset.UtcNow;
            toCreate.UserCreateData = userCreateId;
            toCreate.EntityId = entityId;

            base.Context.Folder.Add(toCreate);
            base.Context.SaveChanges();

            return toCreate;
        }


        public void Delete(Guid folderId, Guid userId, Guid entityId)
        {
            TakeDocModel.Folder folder = base.GetBy(x => x.FolderId == folderId && x.EntityId == entityId).First();
            folder.EtatDeleteData = true;
            folder.UserDeleteData = userId;
            folder.DateDeleteData = System.DateTimeOffset.UtcNow;
            base.Update(folder);
        }

        public TakeDocModel.Folder Update(TakeDocModel.Folder toUpdate, Guid userUpdateId, Guid entityId)
        {
            toUpdate.UserUpdateData = userUpdateId;
            toUpdate.DateUpdateData = System.DateTimeOffset.UtcNow;
            base.Update(toUpdate);
            return toUpdate;
        }
    }
}
