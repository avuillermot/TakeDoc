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
        ICollection<TakeDocModel.TypeDocument> GetBy(Expression<Func<TakeDocModel.TypeDocument, bool>> where, params Expression<Func<TakeDocModel.TypeDocument, object>>[] properties);
        void AddDataField(string typeDocumentRef, string dataFieldRef, bool mandatory, bool delete, int? index, string entityRef, string userRef);
        void AddDataField(Guid typeDocumentId, string dataFieldRef, bool mandatory, bool delete, int? index, Guid entityId, Guid userId);
        void Update(TakeDocModel.TypeDocument type, Guid userId);
        TakeDocModel.TypeDocument Add(string label, Guid entityId, Guid userId);
    }
}
