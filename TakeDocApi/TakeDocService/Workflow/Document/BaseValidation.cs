using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakeDocDataAccess.Document.Interface;
using TakeDocDataAccess.Workflow.Interface;
using Utility.MyUnityHelper;
using System.Transactions;
using TakeDocService.Document.Interface;

namespace TakeDocService.Workflow.Document
{
    public abstract class BaseValidation : BaseService
    {
        private TakeDocModel.TakeDocEntities1 context = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocModel.TakeDocEntities1>();

        protected IDaoDocument daoDocument = UnityHelper.Resolve<IDaoDocument>();
        protected IDaoWorkflow daoWorkflow = UnityHelper.Resolve<IDaoWorkflow>();
        protected TakeDocDataAccess.DaoBase<TakeDocModel.WorkflowType> daoWorkflowType = new TakeDocDataAccess.DaoBase<TakeDocModel.WorkflowType>();
        protected TakeDocDataAccess.DaoBase<TakeDocModel.Status_Document> daoStDocument = new TakeDocDataAccess.DaoBase<TakeDocModel.Status_Document>();

        protected TakeDocDataAccess.DaoBase<TakeDocModel.StatusFolder> daoStFolder = new TakeDocDataAccess.DaoBase<TakeDocModel.StatusFolder>();
        
        
        protected Print.Interface.IReportVersionService servReportVersion = UnityHelper.Resolve<Print.Interface.IReportVersionService>();
        protected IMetaDataService servMeta = UnityHelper.Resolve<IMetaDataService>();
        protected TakeDocService.Workflow.Document.Interface.IStatus servStatus = new TakeDocService.Workflow.Document.Status();

        protected void SetStatus(TakeDocModel.Document document, string status, Guid userId)
        {
            servStatus.SetStatus(document, status, userId, true);
            if (document.DocumentFolderId != null && document.DocumentFolderId != System.Guid.Empty)
            {
                TakeDocModel.StatusFolder stFolder = null;
                if (status == TakeDocModel.Status_Document.Approve || status == TakeDocModel.Status_Document.Archive)
                    stFolder = daoStFolder.GetBy(x => x.EntityId == document.EntityId && x.StatusFolderReference == "CLOSE").First();
                else if (status == TakeDocModel.Status_Document.Create)
                    stFolder = daoStFolder.GetBy(x => x.EntityId == document.EntityId && x.StatusFolderReference == "OPEN").First();
                else if (status == TakeDocModel.Status_Document.Complete || status == TakeDocModel.Status_Document.Incomplete 
                    || status == TakeDocModel.Status_Document.Refuse || status == TakeDocModel.Status_Document.ToValidate)
                    stFolder = daoStFolder.GetBy(x => x.EntityId == document.EntityId && x.StatusFolderReference == "INPROGRESS").First();

                this.context.UpdateFolderStatus(document.DocumentFolderId, document.EntityId, stFolder.StatusFolderId);
            }
        }

        /// <summary>
        /// Add/enable manager validation for this document
        /// </summary>
        /// <param name="user"></param>
        protected void SetManagerValidation(TakeDocModel.Document document, TakeDocModel.UserTk currentUser, string workflowType, int step)
        {
            if (document.DocumentCurrentVersionId.Value == Guid.Empty || document.DocumentCurrentVersionId.Value == null) throw new Exception("Version can't be empty.");
            if (currentUser.UserTkManagerId == Guid.Empty || currentUser.UserTkManagerId == null) throw new Exception("Manager can't be empty.");
            TakeDocModel.Workflow workflow = new TakeDocModel.Workflow();
            workflow.WorkflowId = Guid.NewGuid();
            workflow.EntityId = document.EntityId;
            workflow.WorkflowVersionId = document.DocumentCurrentVersionId.Value;
            workflow.WorkflowIndex = step;
            workflow.WorkflowAnswerId = null;
            workflow.WorkflowTypeId = daoWorkflowType.GetBy(x => x.WorkflowTypeReference == workflowType).First().WorkflowTypeId;
            workflow.WorkflowUserId = currentUser.UserTkManagerId;
            workflow.WorkflowStatusDocumentId = document.DocumentStatusId;
            workflow.EtatDeleteData = false;
            workflow.DateCreateData = System.DateTimeOffset.UtcNow;
            daoWorkflow.Add(workflow);
        }

        /// <summary>
        /// Add/enable manager validation for this document
        /// </summary>
        /// <param name="user"></param>
        protected void SetTypeDocumentValidation(TakeDocModel.Document document, string workflowType, int step)
        {
            if (document.DocumentCurrentVersionId.Value == Guid.Empty || document.DocumentCurrentVersionId.Value == null) throw new Exception("Version can't be empty.");
            TakeDocModel.Workflow workflow = new TakeDocModel.Workflow();
            workflow.WorkflowId = Guid.NewGuid();
            workflow.EntityId = document.EntityId;
            workflow.WorkflowVersionId = document.DocumentCurrentVersionId.Value;
            workflow.WorkflowIndex = step;
            workflow.WorkflowAnswerId = null;
            workflow.WorkflowTypeId = daoWorkflowType.GetBy(x => x.WorkflowTypeReference == workflowType).First().WorkflowTypeId;
            workflow.WorkflowTypeDocumentId = document.DocumentTypeId;
            workflow.WorkflowStatusDocumentId = document.DocumentStatusId;
            workflow.EtatDeleteData = false;
            workflow.DateCreateData = System.DateTimeOffset.UtcNow;
            daoWorkflow.Add(workflow);
        }


        public ICollection<object> GetStatusHistory(Guid documentId, Guid entityId)
        {
            ICollection<object> back = new List<object>();

            try
            {
                TakeDocModel.Document document = daoDocument.GetBy(x => x.DocumentId == documentId && x.EntityId == entityId).First();
                ICollection<TakeDocModel.GetWorkflowHistory_Result> steps = context.GetWorkflowHistory(documentId, entityId).ToList();
                ICollection<TakeDocModel.DocumentStatusHisto> historyStatus = servStatus.GetStatus(documentId, entityId);
                ICollection<TakeDocModel.Status_Document> statusDocument = daoStDocument.GetBy(x => x.EntityId == entityId);
                
                // when document is create
                TakeDocModel.DocumentStatusHisto historyToAdd = new TakeDocModel.DocumentStatusHisto();
                historyToAdd.DateCreateData = document.DateCreateData;
                historyToAdd.DocumentId = document.DocumentId;
                historyToAdd.DocumentIndex = historyStatus.Count() + 1;
                historyToAdd.DocumentStatusId = document.DocumentStatusId;
                historyToAdd.DocumentVersionId = document.DocumentCurrentVersionId.Value;
                historyToAdd.EntityId = document.EntityId;
                historyToAdd.UserCreateData = document.UserCreateData;
                historyStatus.Add(historyToAdd);

                foreach (TakeDocModel.DocumentStatusHisto h in historyStatus)
                {
                    var actions = steps.Where(x => x.StatusDocumentId == h.DocumentStatusId).ToList();
                    ICollection<TakeDocModel.Status_Document> myStatusDocument = statusDocument.Where(x => x.EntityId == entityId && x.StatusDocumentId == h.DocumentStatusId).ToList();
                    string statusLabel = (myStatusDocument.Count() == 0) ? "" : myStatusDocument.First().StatusDocumentLabel;
                    var toAdd = new
                    {
                        DocumentId = h.DocumentId,
                        DocumentIndex = h.DocumentIndex,
                        DocumentStatusId = h.DocumentStatusId,
                        DocumentStatusLabel = statusLabel,
                        DocumentVersionId = h.DocumentVersionId,
                        EntityId = h.EntityId,
                        DateCreateData = h.DateCreateData,
                        Actions = actions
                    };
                    back.Add(toAdd);
                }
            }
            catch (Exception ex)
            {
                base.Logger.Error(ex.Message, ex);
                throw ex;
            }
            return back;
        }
    }
}
