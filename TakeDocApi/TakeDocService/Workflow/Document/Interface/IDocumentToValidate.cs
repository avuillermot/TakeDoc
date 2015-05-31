using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocService.Workflow.Document.Interface
{
    public interface IDocumentToValidate
    {
        ICollection<TakeDocModel.DocumentToValidate> GetByManager(Guid userId);
        ICollection<TakeDocModel.DocumentToValidate> GetByTypeDocument(Guid userId);
        ICollection<TakeDocModel.DocumentToValidate> GetAll(Guid userId);
    }
}
