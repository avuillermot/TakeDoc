using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocService.Workflow.Document.Interface
{
    public interface IAnswer
    {
        void SetAnswer(Guid workflowId, Guid userId, Guid answerId);
    }
}
