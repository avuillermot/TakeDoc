using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocService.Document.Interface
{
    public interface ITypeDocumentService
    {
        ICollection<TakeDocModel.TypeDocument> Get(Guid userId, Guid entityId);
        void AddDataField(string typeDocumentRef, string dataFieldRef, bool mandatory, int? index, string entityRef, string userRef);
    }
}
