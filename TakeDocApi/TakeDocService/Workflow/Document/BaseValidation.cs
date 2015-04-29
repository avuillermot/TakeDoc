using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakeDocDataAccess.Document.Interface;
using Utility.MyUnityHelper;
using System.Transactions;
using TakeDocService.Document.Interface;

namespace TakeDocService.Workflow.Document
{
    public abstract class BaseValidation
    {
        protected IDaoDocument daoDocument = UnityHelper.Resolve<IDaoDocument>();
        protected TakeDocDataAccess.DaoBase<TakeDocModel.Status_Document> daoStDocument = new TakeDocDataAccess.DaoBase<TakeDocModel.Status_Document>();

        protected Print.Interface.IReportVersionService servReportVersion = UnityHelper.Resolve<Print.Interface.IReportVersionService>();
        protected IVersionService servVersion = UnityHelper.Resolve<IVersionService>();

        protected void SetStatus(Guid documentId, string status, Guid userId, bool updateStatusVersion)
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                TakeDocModel.Document document = daoDocument.GetBy(x => x.DocumentId == documentId).First();
                TakeDocModel.Status_Document stDocument = daoStDocument.GetBy(x => x.StatusDocumentReference == status && x.EntityId == x.EntityId).First();

                // mise à jour du statut de la version à recu
                if (document.DocumentCurrentVersionId.HasValue && updateStatusVersion == true)
                {
                    TakeDocModel.Version version = document.Version.Where(x => x.VersionId == document.DocumentCurrentVersionId).First();
                    servVersion.SetStatus(version, status, userId);
                }

                // mise à jour du statut du document à recu
                document.DocumentStatusId = stDocument.StatusDocumentId;
                document.UserUpdateData = userId;
                document.DateUpdateData = System.DateTimeOffset.UtcNow;
                daoDocument.Update(document);

                transaction.Complete();
            }
        }
    }
}
