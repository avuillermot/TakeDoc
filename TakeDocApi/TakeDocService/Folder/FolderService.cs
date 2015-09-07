using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace TakeDocService.Folder
{
    public class FolderService : Interface.IFolderService
    {
        TakeDocDataAccess.Folder.Interface.IDaoFolder daoFolder = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocDataAccess.Folder.Interface.IDaoFolder>();

        public TakeDocModel.Folder Create(JObject jfolder)
        {
            TakeDocModel.Folder folder = new TakeDocModel.Folder();

            Guid entityId = new Guid(jfolder.Value<string>("entityId"));
            Guid userCreateData = new Guid(jfolder.Value<string>("userCreateId"));
            Guid ownerId = new Guid(jfolder.Value<string>("ownerId"));
            Guid folderTypeId = new Guid(jfolder.Value<string>("folderTypId"));

            folder.FolderLabel = jfolder.Value<string>("label");
            folder.FolderTypeId = folderTypeId;
            folder.FolderOwnerId = ownerId;

            if (string.IsNullOrEmpty(jfolder.Value<string>("dateStart")) == false)
            {
                folder.FolderDateStart = DateTimeOffset.Parse(jfolder.Value<string>("dateStart"));
            }

            if (string.IsNullOrEmpty(jfolder.Value<string>("dateEnd")) == false)
            {
                folder.FolderDateEnd = DateTimeOffset.Parse(jfolder.Value<string>("dateEnd"));
            }

            daoFolder.Create(folder, userCreateData, entityId);

            return folder;
        }

        public void Delete(Guid folderId, Guid userId, Guid entityId)
        {
            daoFolder.Delete(folderId, userId, entityId);
        }

        public TakeDocModel.Folder Update(JObject jfolder)
        {
            Guid folderId = new Guid(jfolder.Value<string>("folderId"));
            Guid entityId = new Guid(jfolder.Value<string>("entityId"));
            Guid userUpdateId = new Guid(jfolder.Value<string>("userUpdateId"));
            Guid ownerId = new Guid(jfolder.Value<string>("ownerId"));
            Guid folderTypeId = new Guid(jfolder.Value<string>("folderTypId"));


            TakeDocModel.Folder folder = daoFolder.GetBy(x => x.FolderId == folderId && x.EntityId == entityId).First();

            folder.FolderLabel = jfolder.Value<string>("label");
            folder.FolderOwnerId = ownerId;
            folder.UserUpdateData = userUpdateId;
            folder.DateUpdateData = System.DateTimeOffset.UtcNow;
            
            if (string.IsNullOrEmpty(jfolder.Value<string>("dateStart")) == false)
            {
                folder.FolderDateStart = DateTimeOffset.Parse(jfolder.Value<string>("dateStart"));
            }

            if (string.IsNullOrEmpty(jfolder.Value<string>("dateEnd")) == false)
            {
                folder.FolderDateEnd = DateTimeOffset.Parse(jfolder.Value<string>("dateEnd"));
            }

            daoFolder.Update(folder, userUpdateId, entityId);

            return folder;
        }

        public ICollection<TakeDocModel.Folder> GetByPeriod(Guid ownerId, DateTimeOffset start, DateTimeOffset end)
        {
            ICollection<TakeDocModel.Folder> folders = daoFolder.GetBy(x => x.FolderOwnerId == ownerId
                && x.EtatDeleteData == false
                && (
                    (start <= x.FolderDateStart && x.FolderDateStart <= end)
                    || (end <= x.FolderDateEnd && x.FolderDateEnd <= end)
                    || (start >= x.FolderDateStart && x.FolderDateEnd <= end)
                )
            );
            return folders.ToList();
        }
    }
}
