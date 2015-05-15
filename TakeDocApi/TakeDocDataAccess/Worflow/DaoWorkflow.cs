using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocDataAccess.Workflow
{
    public class DaoWorkflow : DaoBase<TakeDocModel.Workflow>, Interface.IDaoWorkflow
    {
        public bool IsAllApprove(Guid versionId, Guid entityId)
        {
            return !this.Context.Workflow.Any(x => x.WorkflowVersionId == versionId && x.WorkflowEntityId == entityId && x.WorkflowRealize == false);
        }
    }
}
