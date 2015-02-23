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
        TakeDocDataAccess.DaoBase<TakeDocModel.StatutDocument> daoStDocument = new TakeDocDataAccess.DaoBase<TakeDocModel.StatutDocument>();

        IDaoDocument daoDocument = UnityHelper.Resolve<IDaoDocument>();

        Interface.IVersionService servVersion = UnityHelper.Resolve<Interface.IVersionService>();
        Interface.IPageService servPage = UnityHelper.Resolve<Interface.IPageService>();
        Interface.IMetaDataService servMetaData = UnityHelper.Resolve<Interface.IMetaDataService>();

        public TakeDocModel.Document Create(Guid userId, Guid entityId, Guid typeDocumentId, string documentLabel)
        {
            Guid documentId = System.Guid.NewGuid();
            Guid versionId = documentId;
            TakeDocModel.Document document = daoDocument.Create(userId, entityId, documentId, versionId, typeDocumentId, documentLabel);

            servMetaData.CreateMetaData(userId, entityId, document.DocumentId, typeDocumentId);
            TakeDocModel.Version version = servVersion.CreateMajor(userId, entityId, versionId, documentId);

            return document;
        }

        public void AddPage(Guid userId, Guid entityId, Guid documentId, string imageString, string extension)
        {
            TakeDocModel.Document document = daoDocument.GetBy(x => x.DocumentId == documentId).First();
            servPage.Add(userId, entityId, document.DocumentCurrentVersion.Value, imageString, extension);
        }

        public void SetReceive(Guid documentId)
        {
            TakeDocModel.StatutDocument stDocument = daoStDocument.GetBy(x => x.StatutDocumentLibelle == TakeDocModel.StatutVersion.Complete).First();
            TakeDocModel.Document document = daoDocument.GetBy(x => x.DocumentId == documentId).First();

            // mise à jour du statut de la version à recu
            if (document.DocumentCurrentVersion.HasValue)
                servVersion.SetReceive(document.DocumentCurrentVersion.Value);

            // mise à jour du statut du document à recu
            document.DocumentSatutId = stDocument.StatutDocumentId;
            daoDocument.Update(document);
        }

        public void AddVersionMajor(Guid userId, Guid entityId, Guid documentId)
        {
            TakeDocModel.Document document = daoDocument.GetBy(x => x.DocumentId == documentId).First();
            TakeDocModel.Version version = servVersion.CreateMajor(userId, entityId, System.Guid.NewGuid(), documentId);
            document.DocumentCurrentVersion = version.VersionId;
            daoDocument.Update(document);
        }

        public void AddVersionMinor(Guid userId, Guid entityId, Guid documentId)
        {
            TakeDocModel.Document document = daoDocument.GetBy(x => x.DocumentId == documentId).First();
            TakeDocModel.Version version = servVersion.CreateMinor(userId, entityId, System.Guid.NewGuid(), documentId);
            document.DocumentCurrentVersion = version.VersionId;
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
