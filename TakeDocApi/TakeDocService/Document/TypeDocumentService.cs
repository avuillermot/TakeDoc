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
        private TakeDocModel.TakeDocEntities1 context = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocModel.TakeDocEntities1>();

        public ICollection<TakeDocModel.TypeDocument> Get(Guid userId, Guid entityId)
        {
            ICollection<TakeDocModel.TypeDocument> typeDocuments = daoTypeDocument.GetBy(x => x.EntityId == entityId);
            return typeDocuments;
        }

        public void AddDataField(string typeDocumentRef, string dataFieldRef, bool mandatory, int? index, string entityRef, string userRef)
        {
            this.context.AddFieldToDocumentType(typeDocumentRef, dataFieldRef, mandatory, index, entityRef, userRef);
        }
    }
}
