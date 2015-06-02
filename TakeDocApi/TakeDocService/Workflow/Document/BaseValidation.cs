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
    public abstract class BaseValidation
    {
        private TakeDocModel.TakeDocEntities1 context = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocModel.TakeDocEntities1>();

        protected IDaoDocument daoDocument = UnityHelper.Resolve<IDaoDocument>();
        protected IDaoWorkflow daoWorkflow = UnityHelper.Resolve<IDaoWorkflow>();
        protected TakeDocDataAccess.DaoBase<TakeDocModel.WorkflowType> daoWorkflowType = new TakeDocDataAccess.DaoBase<TakeDocModel.WorkflowType>();
        protected TakeDocDataAccess.DaoBase<TakeDocModel.Status_Document> daoStDocument = new TakeDocDataAccess.DaoBase<TakeDocModel.Status_Document>();
        
        
        protected Print.Interface.IReportVersionService servReportVersion = UnityHelper.Resolve<Print.Interface.IReportVersionService>();
        protected IMetaDataService servMeta = UnityHelper.Resolve<IMetaDataService>();
        protected TakeDocService.Workflow.Document.Interface.IStatus servStatus = new TakeDocService.Workflow.Document.Status();

        protected void SetStatus(TakeDocModel.Document document, string status, Guid userId)
        {
            // we update only if status are different
            bool ok = (document.Status_Document.StatusDocumentReference.Equals(status) == false);
            // we update only if the new status is allow
            if (ok == true) ok = servStatus.CheckNewStatus(document.Status_Document.StatusDocumentReference, status);
            if (ok == true) servStatus.SetStatus(document, status, userId, true);
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
            daoWorkflow.Add(workflow);
        }


        public ICollection<object> GetStatusHistory(Guid documentId, Guid entityId)
        {
            ICollection<object> back = new List<object>();

            TakeDocModel.Document document = daoDocument.GetBy(x => x.DocumentId == documentId && x.EntityId == entityId).First();
            ICollection<TakeDocModel.GetWorkflowHistory_Result> historyValidate = context.GetWorkflowHistory(documentId, entityId).ToList();
            ICollection<TakeDocModel.DocumentStatusHisto> historyStatus = servStatus.GetStatus(documentId, entityId);
            ICollection<TakeDocModel.Status_Document> statusDocument = daoStDocument.GetBy(x => x.EntityId == entityId);

            TakeDocModel.DocumentStatusHisto historyToAdd = new TakeDocModel.DocumentStatusHisto();
            historyToAdd.DateCreateData = document.DateUpdateData.Value;
            historyToAdd.DocumentId = document.DocumentId;
            historyToAdd.DocumentIndex = historyStatus.Count() + 1;
            historyToAdd.DocumentStatusId = document.DocumentStatusId;
            historyToAdd.DocumentVersionId = document.DocumentCurrentVersionId.Value;
            historyToAdd.EntityId = document.EntityId;
            historyToAdd.UserCreateData = document.UserUpdateData.Value;
            historyStatus.Add(historyToAdd);

            foreach (TakeDocModel.DocumentStatusHisto h in historyStatus)
            {
                var actions = historyValidate.Where(x => x.StatusDocumentId == h.DocumentStatusId).ToList();
                ICollection<TakeDocModel.Status_Document> myStatusDocument = statusDocument.Where(x => x.EntityId == entityId && x.StatusDocumentId == h.DocumentStatusId).ToList();
                string statusLabel = (myStatusDocument.Count() == 0) ? "" : myStatusDocument.First().StatusDocumentLabel ; 
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
            return back;
        }
    }
}
