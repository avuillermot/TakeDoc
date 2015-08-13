using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocService.Document.Interface
{
    public interface IDocumentCompleteService
    {
        TakeDocModel.Dto.Document.DocumentComplete GetByVersion(Guid versionId, Guid userId, Guid entityId);
        void Update(Guid userId, Guid entityId, string json, bool startWorkflow);
    }
}
