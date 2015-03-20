using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TakeDocDataAccess.Document.Interface;
using Utility.MyUnityHelper;

namespace TakeDocService.Document
{
    public class TypeDocumentService :BaseService, Interface.ITypeDocumentService
    {
        private IDaoTypeDocument daoTypeDocument = UnityHelper.Resolve<IDaoTypeDocument>();

        public ICollection<TakeDocModel.TypeDocument> Get(Guid userId, Guid entityId)
        {
            ICollection<TakeDocModel.TypeDocument> typeDocuments = daoTypeDocument.GetBy(x => x.EntityId == entityId);
            return typeDocuments;
        }

        public ICollection<TakeDocModel.TypeDocument> GetBy(Expression<Func<TakeDocModel.TypeDocument, bool>> where, params Expression<Func<TakeDocModel.TypeDocument, object>>[] properties)
        {
            return daoTypeDocument.GetBy(where, properties);
        }
    }
}
