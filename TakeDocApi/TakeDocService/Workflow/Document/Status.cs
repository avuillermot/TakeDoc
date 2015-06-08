using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TakeDocDataAccess.Document.Interface;
using TakeDocService.Document.Interface;
using Utility.MyUnityHelper;

namespace TakeDocService.Workflow.Document
{
    public class Status : Interface.IStatus
    {
        TakeDocModel.TakeDocEntities1 context = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocModel.TakeDocEntities1>();

        private IDaoDocument daoDocument = UnityHelper.Resolve<IDaoDocument>();
        private TakeDocDataAccess.DaoBase<TakeDocModel.Status_Document> daoStDocument = new TakeDocDataAccess.DaoBase<TakeDocModel.Status_Document>();
        private TakeDocDataAccess.DaoBase<TakeDocModel.DocumentStatusHisto> daoDocHisto = new TakeDocDataAccess.DaoBase<TakeDocModel.DocumentStatusHisto>();

        private IVersionService servVersion = UnityHelper.Resolve<IVersionService>();

        /// <summary>
        /// Check if new status is allow from the current status of document
        /// </summary>
        /// <returns></returns>
        public bool CheckNewStatus(string oldStatus, string newStatus)
        {
            if (newStatus.Equals(TakeDocModel.Status_Document.Create)) return true;

            if (newStatus.Equals(TakeDocModel.Status_Document.Complete))
            {
                if (oldStatus.Equals(TakeDocModel.Status_Document.Create)) return true;
                if (oldStatus.Equals(TakeDocModel.Status_Document.Incomplete)) return true;
                return false;
            }
            else if (newStatus.Equals(TakeDocModel.Status_Document.Incomplete))
            {
                if (oldStatus.Equals(TakeDocModel.Status_Document.Create)) return true;
                return false;
            }
            else if (newStatus.Equals(TakeDocModel.Status_Document.ToValidate))
            {
                if (oldStatus.Equals(TakeDocModel.Status_Document.Complete)) return true;
                //if (oldStatus.Equals(TakeDocModel.Status_Document.Approve)) return true;
                return false;
            }
            else if (newStatus.Equals(TakeDocModel.Status_Document.Approve))
            {
                if (oldStatus.Equals(TakeDocModel.Status_Document.Complete)) return true;
                if (oldStatus.Equals(TakeDocModel.Status_Document.ToValidate)) return true;
                return false;
            }
            else if (newStatus.Equals(TakeDocModel.Status_Document.Refuse))
            {
                if (oldStatus.Equals(TakeDocModel.Status_Document.ToValidate)) return true;
                return false;
            }
            else if (newStatus.Equals(TakeDocModel.Status_Document.Archive))
            {
                if (oldStatus.Equals(TakeDocModel.Status_Document.Complete)) return true;
                if (oldStatus.Equals(TakeDocModel.Status_Document.Approve)) return true;
                return false;
            }

            return false;
        }

        public void SetStatus(Guid documentId, string status, Guid userId, bool updateStatusVersion)
        {
            TakeDocModel.Document document = daoDocument.GetBy(x => x.DocumentId == documentId).First();
            this.SetStatus(document, status, userId, updateStatusVersion);
        }

        public void SetStatus(TakeDocModel.Document document, string status, Guid userId, bool updateStatusVersion)
        {
            bool ok = (document.Status_Document.StatusDocumentReference.Equals(status) == false);
            if (ok)
            {
                bool okStatus = this.CheckNewStatus(document.Status_Document.StatusDocumentReference, status);
                if (okStatus == false) throw new Exception("Erreur workflow");
                using (TransactionScope transaction = new TransactionScope())
                {
                    TakeDocModel.Status_Document stDocument = daoStDocument.GetBy(x => x.StatusDocumentReference == status && x.EntityId == document.EntityId).First();

                    // mise à jour du statut de la version à recu
                    if (document.DocumentCurrentVersionId.HasValue && updateStatusVersion == true)
                    {
                        TakeDocModel.Version version = document.Version.Where(x => x.VersionId == document.DocumentCurrentVersionId).First();
                        servVersion.SetStatus(version, status, userId);
                    }

                    context.AddDocumentStatusHisto(document.DocumentId, document.DocumentStatusId, userId, document.EntityId);
                    // mise à jour du statut du document à recu
                    document.DocumentStatusId = stDocument.StatusDocumentId;
                    document.UserUpdateData = userId;
                    document.DateUpdateData = System.DateTimeOffset.UtcNow;
                    daoDocument.Update(document);

                    transaction.Complete();
                }
            }
        }

        public ICollection<TakeDocModel.DocumentStatusHisto> GetStatus(Guid documentId, Guid entityId)
        {
            return daoDocHisto.GetBy(x => x.DocumentId == documentId && x.EntityId == entityId).OrderBy(x => x.DateCreateData).ToList();
        }

        public ICollection<TakeDocModel.DocumentStatusHisto> GetStatus(TakeDocModel.Document document)
        {
            return this.GetStatus(document.DocumentId, document.EntityId);
        }

        public ICollection<TakeDocModel.Status_Document> GetStatus(Guid entityId)
        {
            return daoStDocument.GetBy(x => x.EntityId == entityId && x.EtatDeleteData == false).ToList();
        }
    }
}
