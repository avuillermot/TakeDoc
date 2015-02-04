using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocDataAccess.Document
{
    public class DaoTypeDocument : DaoBase<TakeDocModel.Type_Document>, Interface.IDaoTypeDocument
    {
        public ICollection<TakeDocModel.Type_Document> Get(Guid userId, Guid entityId)
        {
            return this.Context.Type_Document.Where(x => x.EntityId == entityId).ToList();
        }
    }
}
