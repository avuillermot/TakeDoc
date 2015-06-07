using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakeDocDataAccess.Workflow.Interface;
using Utility.MyUnityHelper;

namespace TakeDocService.Workflow.Document
{
    public class Answer : BaseService, Interface.IAnswer
    {
        IDaoWorkflow daoWf = UnityHelper.Resolve<IDaoWorkflow>();
        
        public void SetAnswer(Guid workflowId, Guid userId, Guid answerId)
        {
            try
            {
                TakeDocModel.Workflow workflow = daoWf.GetBy(x => x.WorkflowId == workflowId && x.WorkflowDateRealize == null && x.WorkflowAnswerId == null).First();
                daoWf.SetAnswer(workflow, answerId, userId);
            }
            catch (Exception ex)
            {
                base.Logger.Error("Answer.Answer", ex);
            }
        }
    }
}
