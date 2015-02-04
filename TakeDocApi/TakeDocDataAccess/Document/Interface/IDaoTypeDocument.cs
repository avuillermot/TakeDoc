using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocDataAccess.Document.Interface
{
    public interface IDaoTypeDocument
    {
        ICollection<TakeDocModel.Type_Document> Get(Guid userId, Guid entityId);
    }
}
