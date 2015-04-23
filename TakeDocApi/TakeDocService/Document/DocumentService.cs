using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TakeDocDataAccess.Document.Interface;
using TakeDocDataAccess.Document;
using Utility.MyUnityHelper;
using System.Transactions;

namespace TakeDocService.Document
{
    public class DocumentService : BaseService, Interface.IDocumentService
    {
        TakeDocDataAccess.DaoBase<TakeDocModel.Status_Document> daoStDocument = new TakeDocDataAccess.DaoBase<TakeDocModel.Status_Document>();

        IDaoDocument daoDocument = UnityHelper.Resolve<IDaoDocument>();

        Interface.IVersionService servVersion = UnityHelper.Resolve<Interface.IVersionService>();
        Interface.IMetaDataService servMeta = UnityHelper.Resolve<Interface.IMetaDataService>();
        Interface.IPageService servPage = UnityHelper.Resolve<Interface.IPageService>();
        
        public TakeDocModel.Document Create(Guid userId, Guid entityId, Guid typeDocumentId, string documentLabel)
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                this.Logger.Info(string.Format("Create document by {0}", userId));
                Guid documentId = System.Guid.NewGuid();
                Guid versionId = documentId;
                TakeDocModel.Document document = daoDocument.Create(userId, entityId, documentId, versionId, typeDocumentId, documentLabel);

                TakeDocModel.Version version = servVersion.CreateMajor(userId, entityId, versionId, documentId, typeDocumentId);

                transaction.Complete();
                return document;
            }
        }

        public void AddPage(Guid userId, Guid entityId, Guid documentId, string imageString, string extension, int rotation)
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                TakeDocModel.Document document = daoDocument.GetBy(x => x.DocumentId == documentId).First();
                servPage.Add(userId, entityId, document.DocumentCurrentVersionId.Value, imageString, extension, rotation);

                transaction.Complete();
            }
        }

        public void SetStatus(Guid documentId, string status, Guid userId, bool updateStatusVersion)
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

        public void AddVersionMajor(Guid userId, Guid entityId, Guid documentId)
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                TakeDocModel.Document document = daoDocument.GetBy(x => x.DocumentId == documentId).First();
                TakeDocModel.Version version = servVersion.CreateMajor(userId, entityId, System.Guid.NewGuid(), documentId, document.DocumentTypeId);
                this.SetStatus(document.DocumentId, TakeDocModel.Status_Document.Create, userId, false);
                document.DocumentCurrentVersionId = version.VersionId;
                daoDocument.Update(document);

                transaction.Complete();
            }
        }

        public void AddVersionMinor(Guid userId, Guid entityId, Guid documentId)
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                TakeDocModel.Document document = daoDocument.GetBy(x => x.DocumentId == documentId).First();
                TakeDocModel.Version version = servVersion.CreateMinor(userId, entityId, System.Guid.NewGuid(), documentId, document.DocumentTypeId);
                this.SetStatus(document.DocumentId, TakeDocModel.Status_Document.Create, userId, false);
                document.DocumentCurrentVersionId = version.VersionId;
                daoDocument.Update(document);

                transaction.Complete();
            }
        }

        public TakeDocModel.Document GetById(Guid documentId, params System.Linq.Expressions.Expression<Func<TakeDocModel.Document, object>>[] properties)
        {
            ICollection<TakeDocModel.Document> documents = daoDocument.GetBy(x => x.DocumentId == documentId, properties);
            if (documents.Count() == 0) return null;
            return documents.First();
        }

        public void SetMetaData(Guid userId, Guid entityId, Guid versionId, IDictionary<string, string> metadatas)
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                TakeDocModel.Document document = daoDocument.GetBy(x => x.DocumentCurrentVersionId == versionId).First();

                TakeDocModel.Version version = document.Version.Where(x => x.VersionId == document.DocumentCurrentVersionId).First();
                servMeta.SetMetaData(userId, entityId, versionId, metadatas);

                if (TakeDocModel.Status_Version.Incomplete == version.Status_Version.StatusVersionReference) servVersion.SetStatus(version, TakeDocModel.Status_Version.Complete, userId);
                if (TakeDocModel.Status_Document.Incomplete == document.Status_Document.StatusDocumentReference)  this.SetStatus(document.DocumentId, TakeDocModel.Status_Document.Complete, userId, true);

                transaction.Complete();
            }
        }

        public void Delete(Guid documentId, Guid entityId, Guid userId)
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                TakeDocModel.Document document = daoDocument.GetBy(x => x.DocumentId == documentId).First();
                servVersion.Delete(documentId, entityId, userId);
                document.EtatDeleteData = true;
                document.DateDeleteData = System.DateTime.UtcNow;
                document.UserDeleteData = userId;
                daoDocument.Update(document);

                transaction.Complete();
            }
        }
   }
}
