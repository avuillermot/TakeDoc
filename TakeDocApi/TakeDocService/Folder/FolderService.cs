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
            Guid folderTypeId = new Guid(jfolder.Value<string>("folderTypeId"));

            folder.FolderLabel = jfolder.Value<string>("title");
            folder.FolderTypeId = folderTypeId;
            folder.FolderOwnerId = ownerId;

            folder.FolderDateStart = DateTimeOffset.Parse(jfolder.Value<string>("start"),System.Globalization.CultureInfo.InvariantCulture);
            folder.FolderDateEnd = DateTimeOffset.Parse(jfolder.Value<string>("end"),System.Globalization.CultureInfo.InvariantCulture);

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

            TakeDocModel.Folder folder = daoFolder.GetBy(x => x.FolderId == folderId && x.EntityId == entityId).First();

            folder.FolderLabel = jfolder.Value<string>("title");
            folder.FolderOwnerId = ownerId;
            folder.UserUpdateData = userUpdateId;
            folder.DateUpdateData = System.DateTimeOffset.UtcNow;
            
            if (string.IsNullOrEmpty(jfolder.Value<string>("start")) == false)
            {
                folder.FolderDateStart = DateTimeOffset.Parse(jfolder.Value<string>("start"), System.Globalization.CultureInfo.InvariantCulture);
            }

            if (string.IsNullOrEmpty(jfolder.Value<string>("end")) == false)
            {
                folder.FolderDateEnd = DateTimeOffset.Parse(jfolder.Value<string>("end"), System.Globalization.CultureInfo.InvariantCulture);
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
