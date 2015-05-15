using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocDataAccess.Workflow.Interface
{
    public interface IDaoWorkflow
    {
        /// <summary>
        /// Check if all required validation are ok
        /// </summary>
        /// <param name="VersionId"></param>
        /// <returns></returns>
        bool IsAllApprove(Guid VersionId, Guid entityId);
       
    }
}
