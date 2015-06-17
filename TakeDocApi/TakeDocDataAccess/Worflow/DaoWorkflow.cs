using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocDataAccess.Workflow
{
    public class DaoWorkflow : DaoBase<TakeDocModel.Workflow>, Interface.IDaoWorkflow
    {
        public bool IsAllApprove(Guid versionId, Guid entityId)
        {
            return !this.Context.Workflow.Any(x => x.WorkflowVersionId == versionId && x.EntityId == entityId && x.WorkflowAnswerId == null);
        }
        
        public void Add(TakeDocModel.Workflow workflow)
        {
            this.Context.Workflow.Add(workflow);
            this.Context.SaveChanges();
        }

        new public ICollection<TakeDocModel.Workflow> GetBy(Expression<Func<TakeDocModel.Workflow, bool>> where, params Expression<Func<TakeDocModel.Workflow, object>>[] properties)
        {
            return base.GetBy(where, properties);
        }

        public void SetAnswer(TakeDocModel.Workflow workflow, Guid answerId, Guid userId, string comment) {
            workflow.WorkflowAnswerId = answerId;
            if (workflow.WorkflowUserId == null) workflow.WorkflowUserId = userId;
            workflow.WorkflowDateRealize = System.DateTime.UtcNow;
            workflow.WorkflowComment = comment;
            this.Update(workflow);
        }

        public void CancelWorkflowStep(Guid versionId)
        {
            ICollection<TakeDocModel.Workflow> workflows = this.GetBy(x => x.WorkflowVersionId == versionId && x.WorkflowAnswerId == null && x.EtatDeleteData == false);
            foreach (TakeDocModel.Workflow workflow in workflows)
            {
                workflow.EtatDeleteData = true;
                this.Update(workflow);
            }
        }
    }
}
