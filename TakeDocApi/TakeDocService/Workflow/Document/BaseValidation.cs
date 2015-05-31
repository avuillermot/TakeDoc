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
            workflow.WorkflowRealize = false;
            workflow.WorkflowTypeId = daoWorkflowType.GetBy(x => x.WorkflowTypeReference == workflowType && x.EntityId == document.EntityId).First().WorkflowTypeId;
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
            workflow.WorkflowRealize = false;
            workflow.WorkflowTypeId = daoWorkflowType.GetBy(x => x.WorkflowTypeReference == workflowType && x.EntityId == document.EntityId).First().WorkflowTypeId;
            workflow.WorkflowTypeDocumentId = document.DocumentTypeId;
            workflow.WorkflowStatusDocumentId = document.DocumentStatusId;
            daoWorkflow.Add(workflow);
        }


        public ICollection<object> GetStatusHistory(Guid documentId, Guid entityId)
        {
            var req1 = from histo in context.DocumentStatusHisto
                      join doc in context.Document on histo.DocumentVersionId equals doc.DocumentCurrentVersionId
                      join status in context.Status_Document on histo.DocumentStatusId equals status.StatusDocumentId
                      join wo in context.Workflow on histo.DocumentVersionId equals wo.WorkflowVersionId into wohisto
                      where doc.DocumentId == documentId && histo.EntityId == entityId && status.EntityId == entityId && histo.DocumentId == documentId
                      select new { 
                          Status = histo.DocumentStatusId, StatusLabel = status.StatusDocumentLabel, StatusReference = status.StatusDocumentReference,
                          Date = histo.DateCreateData,
                          Validate = wohisto
                      };
            return req1.ToList<object>();
        }
    }
}
