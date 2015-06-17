using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakeDocDataAccess.Workflow.Interface;
using TakeDocDataAccess.Document.Interface;
using Utility.MyUnityHelper;
using System.Transactions;

namespace TakeDocService.Workflow.Document
{
    public class Answer : BaseService, Interface.IAnswer
    {
        IDaoWorkflow daoWf = UnityHelper.Resolve<IDaoWorkflow>();
        IDaoDocument daoDoc = UnityHelper.Resolve<IDaoDocument>();
        
        TakeDocDataAccess.DaoBase<TakeDocModel.WorkflowAnswer> daoAnswer = new TakeDocDataAccess.DaoBase<TakeDocModel.WorkflowAnswer>();

        protected TakeDocService.Workflow.Document.Interface.IStatus servStatus = new TakeDocService.Workflow.Document.Status();

        public void SetAnswer(Guid workflowId, Guid versionId, Guid userId, Guid answerId, string comment)
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                try
                {
                    TakeDocModel.Document document = daoDoc.GetBy(x => x.DocumentCurrentVersionId == versionId).First();
                    ICollection<TakeDocModel.Workflow> workflows = daoWf.GetBy(x => x.WorkflowVersionId == versionId, x => x.WorkflowAnswer);
                    TakeDocModel.Workflow workflow = workflows.Where(x => x.WorkflowId == workflowId && x.WorkflowDateRealize == null && x.WorkflowAnswerId == null).First();

                    daoWf.SetAnswer(workflow, answerId, userId, comment);
                    TakeDocModel.WorkflowAnswer answer = daoAnswer.GetBy(x => x.WorkflowAnswerId == answerId).First();
                    if (answer.WorkflowAnswerGoOn == false)
                    {
                        daoWf.CancelWorkflowStep(document.DocumentCurrentVersionId.Value);
                        servStatus.SetStatus(document.DocumentId, TakeDocModel.Status_Document.Refuse, userId, true);
                    }
                    else
                    {
                        bool isAllApprove = !workflows.Any(x => x.WorkflowAnswer == null);
                        if (isAllApprove) isAllApprove = !workflows.Any(x => x.WorkflowAnswer.WorkflowAnswerGoOn != true);
                        if (isAllApprove) servStatus.SetStatus(document.DocumentId, TakeDocModel.Status_Document.Approve, userId, true);
                    }
                    transaction.Complete();
                }
                catch (Exception ex)
                {
                    base.Logger.Error("Answer.Answer", ex);
                    throw ex;
                }
            }
        }
    }
}
