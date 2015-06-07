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

        public void SetAnswer(TakeDocModel.Workflow workflow, Guid answerId, Guid userId) {
            workflow.WorkflowAnswerId =answerId;
            if (workflow.WorkflowUserId == null) workflow.WorkflowUserId = userId;
            workflow.WorkflowDateRealize = System.DateTime.UtcNow;
            this.Update(workflow);
        }
    }
}
