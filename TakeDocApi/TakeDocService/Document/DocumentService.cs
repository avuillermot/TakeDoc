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
        Interface.IPageService servPage = UnityHelper.Resolve<Interface.IPageService>();
        
        public TakeDocModel.Document Create(Guid userId, Guid entityId, Guid typeDocumentId, string documentLabel)
        {
            this.Logger.Info(string.Format("Create document by {0}", userId));
            Guid documentId = System.Guid.NewGuid();
            Guid versionId = documentId;
            TakeDocModel.Document document = daoDocument.Create(userId, entityId, documentId, versionId, typeDocumentId, documentLabel);

            TakeDocModel.Version version = servVersion.CreateMajor(userId, entityId, versionId, documentId, typeDocumentId);

            return document;
        }

        public void AddPage(Guid userId, Guid entityId, Guid documentId, string imageString, string extension, int rotation)
        {
            TakeDocModel.Document document = daoDocument.GetBy(x => x.DocumentId == documentId).First();
            servPage.Add(userId, entityId, document.DocumentCurrentVersionId.Value, imageString, extension, rotation);
        }

        public void SetStatusSend(Guid documentId)
        {
            TakeDocModel.Document document = daoDocument.GetBy(x => x.DocumentId == documentId).First();
            TakeDocModel.Status_Document stDocument = daoStDocument.GetBy(x => x.StatusDocumentReference == TakeDocModel.Status_Document.Send && x.EntityId == x.EntityId).First();
            
            // mise à jour du statut de la version à recu
            if (document.DocumentCurrentVersionId.HasValue)
                servVersion.SetStatusSend(document.DocumentCurrentVersionId.Value, document.EntityId);

            // mise à jour du statut du document à recu
            document.DocumentStatusId = stDocument.StatusDocumentId;
            daoDocument.Update(document);
        }

        public void AddVersionMajor(Guid userId, Guid entityId, Guid documentId)
        {
            TakeDocModel.Document document = daoDocument.GetBy(x => x.DocumentId == documentId).First();
            TakeDocModel.Version version = servVersion.CreateMajor(userId, entityId, System.Guid.NewGuid(), documentId, document.DocumentTypeId);
            document.DocumentCurrentVersionId = version.VersionId;
            daoDocument.Update(document);
        }

        public void AddVersionMinor(Guid userId, Guid entityId, Guid documentId)
        {
            TakeDocModel.Document document = daoDocument.GetBy(x => x.DocumentId == documentId).First();
            TakeDocModel.Version version = servVersion.CreateMinor(userId, entityId, System.Guid.NewGuid(), documentId, document.DocumentTypeId);
            document.DocumentCurrentVersionId = version.VersionId;
            daoDocument.Update(document);
        }

        public TakeDocModel.Document GetById(Guid documentId, params System.Linq.Expressions.Expression<Func<TakeDocModel.Document, object>>[] properties)
        {
            ICollection<TakeDocModel.Document> documents = daoDocument.GetBy(x => x.DocumentId == documentId, properties);
            if (documents.Count() == 0) return null;
            return documents.First();
        }
   }
}
