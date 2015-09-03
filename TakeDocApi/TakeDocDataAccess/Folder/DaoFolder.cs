using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
