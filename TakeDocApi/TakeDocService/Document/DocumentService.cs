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
        IDaoDocument daoDocument = UnityHelper.Resolve<IDaoDocument>();
        TakeDocDataAccess.Security.Interface.IDaoUserTk daoUser = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocDataAccess.Security.Interface.IDaoUserTk>();
        
        Interface.IVersionService servVersion = UnityHelper.Resolve<Interface.IVersionService>();
        Interface.IPageService servPage = UnityHelper.Resolve<Interface.IPageService>();
        TakeDocService.Workflow.Document.Interface.IStatus servStatus = new TakeDocService.Workflow.Document.Status();
        Interface.IMetaDataService servMeta = UnityHelper.Resolve<Interface.IMetaDataService>();
                
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

        public void AddVersionMajor(Guid userId, Guid entityId, Guid documentId)
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                TakeDocModel.Document document = daoDocument.GetBy(x => x.DocumentId == documentId).First();
                TakeDocModel.Version version = servVersion.CreateMajor(userId, entityId, System.Guid.NewGuid(), documentId, document.DocumentTypeId);
                servStatus.SetStatus(document.DocumentId, TakeDocModel.Status_Document.Create, userId, false);
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
                servStatus.SetStatus(document.DocumentId, TakeDocModel.Status_Document.Create, userId, false);
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
                TakeDocModel.UserTk user = daoUser.GetBy(x => x.UserTkId == userId).First();

                servMeta.SetMetaData(userId, entityId, versionId, metadatas);
                //***********************************
                // update status of document
                //***********************************
                TakeDocModel.Type_Validation validation = version.Document.Type_Document.Type_Validation;

                TakeDocService.Workflow.Document.Interface.IValidation wfValidation = null;
                if (validation.TypeValidationReference == "AUTO")
                {
                    wfValidation = new TakeDocService.Workflow.Document.ValidationAuto();
                    wfValidation.Execute(document, user);
                }
                else if (validation.TypeValidationReference == "NO")
                {
                    wfValidation = new TakeDocService.Workflow.Document.ValidationNo();
                    wfValidation.Execute(document, user);
                }
                else if (validation.TypeValidationReference == "MANAGER")
                {
                    wfValidation = new TakeDocService.Workflow.Document.ValidationManager();
                    wfValidation.Execute(document, user);
                }
                else if (validation.TypeValidationReference == "BACKOFFICE")
                {
                    wfValidation = new TakeDocService.Workflow.Document.ValidationBackOffice();
                    wfValidation.Execute(document, user);
                }
                else if (validation.TypeValidationReference == "MANAGER-BACKOFFICE")
                {
                    wfValidation = new TakeDocService.Workflow.Document.ValidationManagerBackOffice();
                    wfValidation.Execute(document, user);
                }

                //***********************************
                // end update status of document
                //***********************************

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
