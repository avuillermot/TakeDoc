using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TakeDocDataAccess.Document.Interface;
using TakeDocDataAccess.Document;
using Utility.MyUnityHelper;
using System.Transactions;
using Newtonsoft.Json.Linq;

namespace TakeDocService.Document
{
    public class DocumentService : BaseService, Interface.IDocumentService
    {
        IDaoDocument daoDocument = UnityHelper.Resolve<IDaoDocument>();
        TakeDocDataAccess.Security.Interface.IDaoUserTk daoUser = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocDataAccess.Security.Interface.IDaoUserTk>();
        TakeDocDataAccess.Document.Interface.IDaoView_DocumentExtended daoDocExtended = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocDataAccess.Document.Interface.IDaoView_DocumentExtended>();
        
        Interface.IVersionService servVersion = UnityHelper.Resolve<Interface.IVersionService>();
        Interface.IPageService servPage = UnityHelper.Resolve<Interface.IPageService>();
        TakeDocService.Workflow.Document.Interface.IStatus servStatus = new TakeDocService.Workflow.Document.Status();
        Interface.IMetaDataService servMeta = UnityHelper.Resolve<Interface.IMetaDataService>();
                
        public TakeDocModel.Document Create(Guid userId, Guid entityId, Guid typeDocumentId, string documentLabel)
        {
            return this.Create(userId, entityId, typeDocumentId, documentLabel, null);
        }

        public TakeDocModel.Document Create(Guid userId, Guid entityId, Guid typeDocumentId, string documentLabel, Guid? folderId)
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                this.Logger.Info(string.Format("Create document by {0}", userId));
                Guid documentId = System.Guid.NewGuid();
                Guid versionId = documentId;
                TakeDocModel.Document document = daoDocument.Create(userId, entityId, documentId, versionId, typeDocumentId, documentLabel, folderId);

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

        public void Update(TakeDocModel.UserTk user, TakeDocModel.Entity entity, TakeDocModel.Version version, Newtonsoft.Json.Linq.JObject jsonDocument, Newtonsoft.Json.Linq.JArray jsonMetadata, bool startWorkflow)
        {
            TakeDocModel.Document document = daoDocument.GetBy(x => x.DocumentCurrentVersionId == version.VersionId).First();

            if (jsonDocument != null && jsonDocument.Value<string>("label") != null) this.SetTitle(jsonDocument.Value<string>("label"), version.VersionId, user.UserTkId, entity.EntityId);
            if (jsonMetadata != null) servMeta.SetMetaData(user.UserTkId, document, entity, jsonMetadata);

            // if there is exception, all metadata are ok -> so document is complete
            if (jsonDocument != null) servStatus.SetStatus(document, TakeDocModel.Status_Document.Complete, user.UserTkId, true);
            if (startWorkflow) this.StartWorkflow(document, user, entity.EntityId, false);
        }

        private void StartWorkflow(TakeDocModel.Document document, TakeDocModel.UserTk user, Guid entityId, bool checkValue)
        {
            bool ok = true;
            if (checkValue)
            {
                TakeDocModel.Version version = servVersion.GetById(document.DocumentCurrentVersionId.Value, x => x.MetaData);
                ok = servMeta.BeProven(document, version.MetaData);
                // here, all metadata are ok, so document is complete
                servStatus.SetStatus(document, TakeDocModel.Status_Document.Complete, user.UserTkId, true);
            }
            if (ok)
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    //***********************************
                    // update status of document
                    //***********************************
                    TakeDocModel.WorkflowType validation = document.Type_Document.WorkflowType;

                    TakeDocService.Workflow.Document.Interface.IValidation wfValidation = null;
                    if (validation.WorkflowTypeReference == "AUTO")
                    {
                        wfValidation = new TakeDocService.Workflow.Document.ValidationAuto();
                        wfValidation.Execute(document, user);
                    }
                    else if (validation.WorkflowTypeReference == "NO")
                    {
                        wfValidation = new TakeDocService.Workflow.Document.ValidationNo();
                        wfValidation.Execute(document, user);
                    }
                    else if (validation.WorkflowTypeReference == "MANAGER")
                    {
                        wfValidation = new TakeDocService.Workflow.Document.ValidationManager();
                        wfValidation.Execute(document, user);
                    }
                    else if (validation.WorkflowTypeReference == "BACKOFFICE")
                    {
                        wfValidation = new TakeDocService.Workflow.Document.ValidationBackOffice();
                        wfValidation.Execute(document, user);
                    }
                    else if (validation.WorkflowTypeReference == "MANAGER-BACKOFFICE")
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
                daoDocument.Delete(document);
                transaction.Complete();
            }
        }

        public void DeleteByFolderId(Guid folderId, Guid entityId, Guid userId)
        {
            TakeDocModel.Document document = daoDocument.GetBy(x => x.DocumentFolderId == folderId).First();
            this.Delete(document.DocumentId, entityId, userId);
        }

        public ICollection<TakeDocModel.View_DocumentExtended> Search(string title, string reference, Guid typeDocumentId, ICollection<TakeDocModel.Dto.Document.SearchMetadata> searchs, Guid userId, Guid entityId)
        {
            return daoDocExtended.Search(title, reference, typeDocumentId, searchs, userId, entityId);
        }

        public void SetTitle(string title, Guid versionId, Guid userId, Guid entityId)
        {
            TakeDocModel.Document document = daoDocument.GetBy(x => x.DocumentCurrentVersionId == versionId && x.EntityId == entityId).First();
            document.DateUpdateData = System.DateTime.UtcNow;
            document.UserUpdateData = userId;
            document.DocumentLabel = title;
            daoDocument.Update(document);
        }
   }
}
