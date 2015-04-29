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
        TakeDocModel.TakeDocEntities1 context = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocModel.TakeDocEntities1>();

        protected IDaoDocument daoDocument = UnityHelper.Resolve<IDaoDocument>();
        protected TakeDocDataAccess.DaoBase<TakeDocModel.Status_Document> daoStDocument = new TakeDocDataAccess.DaoBase<TakeDocModel.Status_Document>();

        protected Print.Interface.IReportVersionService servReportVersion = UnityHelper.Resolve<Print.Interface.IReportVersionService>();
        protected IVersionService servVersion = UnityHelper.Resolve<IVersionService>();
        protected IMetaDataService servMeta = UnityHelper.Resolve<IMetaDataService>();

        protected void SetStatus(TakeDocModel.Document document, string status, Guid userId, IDictionary<string, string> metadatas)
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                servMeta.SetMetaData(userId, document.EntityId, document.DocumentCurrentVersionId.Value, metadatas);
                
                TakeDocModel.Status_Document stDocument = daoStDocument.GetBy(x => x.StatusDocumentReference == status && x.EntityId == x.EntityId).First();

                // mise à jour du statut de la version à recu
                if (document.DocumentCurrentVersionId.HasValue)
                {
                    TakeDocModel.Version version = document.Version.Where(x => x.VersionId == document.DocumentCurrentVersionId).First();
                    servVersion.SetStatus(version, status, userId);
                }

                context.AddDocumentStatus(document.DocumentId, document.DocumentStatusId, userId, document.EntityId);
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
