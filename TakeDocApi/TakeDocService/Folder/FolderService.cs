using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Transactions;

namespace TakeDocService.Folder
{
    public class FolderService : Interface.IFolderService
    {
        TakeDocDataAccess.DaoBase<TakeDocModel.StatusFolder> daoStatusFolder = new TakeDocDataAccess.DaoBase<TakeDocModel.StatusFolder>();
        TakeDocDataAccess.DaoBase<TakeDocModel.FolderType> daoFolderType = new TakeDocDataAccess.DaoBase<TakeDocModel.FolderType>();

        TakeDocDataAccess.Folder.Interface.IDaoFolder daoFolder = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocDataAccess.Folder.Interface.IDaoFolder>();
        TakeDocDataAccess.Document.Interface.IDaoTypeDocument daoTypeDoc = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocDataAccess.Document.Interface.IDaoTypeDocument>();
        TakeDocDataAccess.Parameter.Interface.IDaoEntity daoEntity = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocDataAccess.Parameter.Interface.IDaoEntity>();

        TakeDocService.Document.Interface.IDocumentService servDocument = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocService.Document.Interface.IDocumentService>();

        public TakeDocModel.Folder Create(JObject jfolder)
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                TakeDocModel.Folder folder = new TakeDocModel.Folder();

                Guid entityId = new Guid(jfolder.Value<string>("entityId"));
                Guid userCreateData = new Guid(jfolder.Value<string>("userCreateId"));
                Guid ownerId = new Guid(jfolder.Value<string>("ownerId"));
                Guid folderTypeId = new Guid(jfolder.Value<string>("folderTypeId"));

                TakeDocModel.StatusFolder status = daoStatusFolder.GetBy(x => x.StatusFolderReference == "OPEN" && x.EntityId == entityId).First();
                TakeDocModel.TypeDocument typeDoc = daoTypeDoc.GetBy(x => x.FolderTypeId == folderTypeId).First();

                folder.FolderLabel = jfolder.Value<string>("title");
                folder.FolderTypeId = folderTypeId;
                folder.FolderOwnerId = ownerId;
                folder.FolderStatusId = status.StatusFolderId;

                folder.FolderDateStart = DateTimeOffset.Parse(jfolder.Value<string>("start"), System.Globalization.CultureInfo.InvariantCulture);
                folder.FolderDateEnd = DateTimeOffset.Parse(jfolder.Value<string>("end"), System.Globalization.CultureInfo.InvariantCulture);

                daoFolder.Create(folder, userCreateData, entityId);
                servDocument.Create(ownerId, folder.EntityId, typeDoc.TypeDocumentId, folder.FolderLabel, folder.FolderId);
                transaction.Complete();
                return folder;
            }
        }

        public void Delete(Guid folderId, Guid userId, Guid entityId)
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                daoFolder.Delete(folderId, userId, entityId);
                servDocument.DeleteByFolderId(folderId, entityId, userId);
                transaction.Complete();
            }
        }

        public TakeDocModel.Folder Update(JObject jfolder)
        {
            Guid folderId = new Guid(jfolder.Value<string>("folderId"));
            Guid entityId = new Guid(jfolder.Value<string>("entityId"));
            Guid userUpdateId = new Guid(jfolder.Value<string>("userUpdateId"));
            Guid ownerId = new Guid(jfolder.Value<string>("ownerId"));

            TakeDocModel.Folder folder = daoFolder.GetBy(x => x.FolderId == folderId && x.EntityId == entityId).First();

            folder.FolderLabel = jfolder.Value<string>("title");
            folder.FolderDetail = jfolder.Value<string>("detail");
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

        public ICollection<TakeDocModel.Folder> GetByPeriod(ICollection<Guid> agendas, DateTimeOffset start, DateTimeOffset end)
        {
            ICollection<TakeDocModel.Folder> folders = daoFolder.GetBy(x => agendas.Contains(x.FolderOwnerId)
                && x.EtatDeleteData == false
                && (
                    (start <= x.FolderDateStart && x.FolderDateStart <= end)
                    || (end <= x.FolderDateEnd && x.FolderDateEnd <= end)
                    || (start >= x.FolderDateStart && x.FolderDateEnd <= end)
                )
            );
            return folders.ToList();
        }

        public ICollection<object> GetJsonByPeriod(ICollection<JObject> agendas, DateTimeOffset start, DateTimeOffset end)
        {
            ICollection<Guid> agendaIds = new List<Guid>();
            foreach (JObject agenda in agendas) agendaIds.Add(new Guid(agenda.Value<string>("id")));

            ICollection<TakeDocModel.Entity> entitys = daoEntity.GetAll();
            ICollection<TakeDocModel.FolderType> types = daoFolderType.GetAll();
            ICollection<TakeDocModel.StatusFolder> status = daoStatusFolder.GetAll();

            ICollection<TakeDocModel.Folder> folders = this.GetByPeriod(agendaIds, start, end);
            IList<object> items = new List<object>();

            foreach (TakeDocModel.Folder folder in folders)
            {
                TakeDocModel.Entity entity = entitys.First(x => x.EntityId == folder.EntityId);
                TakeDocModel.FolderType ctype = types.First(x => x.FolderTypeId == folder.FolderTypeId);
                TakeDocModel.StatusFolder cstatus = status.First(x => x.StatusFolderId == folder.FolderStatusId);

                string color = string.Empty;
                ICollection<JObject> currentAgenda = agendas.Where(x => new Guid(x.Value<string>("id")) == folder.FolderOwnerId).ToList();
                if (currentAgenda.Count() > 0) color = currentAgenda.First().Value<string>("color");
                var json = new
                {
                    id = folder.FolderId,
                    folderId = folder.FolderId,
                    folderTypeLabel = ctype.FolderTypeLabel,
                    title = folder.FolderLabel,
                    detail = folder.FolderDetail,
                    status = cstatus.StatusFolderLabel,
                    start = folder.FolderDateStart,
                    end = folder.FolderDateEnd,
                    allDay = false,
                    entityId = folder.EntityId,
                    entityLabel = entity.EntityLabel,
                    ownerId = folder.FolderOwnerId,
                    folderTypeId = folder.FolderTypeId,
                    readOnly = false,
                    color = color,
                    documentId = folder.Document.First().DocumentId,
                    documentVersionId = folder.Document.First().DocumentCurrentVersionId,
                    documentReference = folder.Document.First().DocumentReference,
                    typeDocumentId = folder.Document.First().DocumentTypeId,
                    documentStatutReference = folder.Document.First().Status_Document.StatusDocumentReference

                };
                items.Add(json);
            }

            return items;
        }
    }
}
