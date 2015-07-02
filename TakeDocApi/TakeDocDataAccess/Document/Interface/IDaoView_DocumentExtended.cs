using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocDataAccess.Document.Interface
{
    public interface IDaoView_DocumentExtended
    {
        ICollection<TakeDocModel.View_DocumentExtended> Search(Guid typeDocumentId, ICollection<TakeDocModel.Dto.Document.SearchMetadata> metadatas, Guid userId, Guid entityId);
    }
}
