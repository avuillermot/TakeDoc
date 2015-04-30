using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocService.Workflow.Document.Interface
{
    public interface IValidation
    {
        bool Execute(TakeDocModel.Document document, TakeDocModel.UserTk user);
    }
}
