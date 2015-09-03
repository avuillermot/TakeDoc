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
    }
}
