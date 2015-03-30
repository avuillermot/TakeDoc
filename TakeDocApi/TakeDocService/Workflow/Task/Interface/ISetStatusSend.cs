using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocService.Workflow.Task.Interface
{
    public interface ISetStatusSend
    {
        void Execute(Guid userId);
    }
}
