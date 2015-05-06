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
        private TakeDocModel.TakeDocEntities1 context = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocModel.TakeDocEntities1>();

        private IDaoTypeDocument daoTypeDocument = UnityHelper.Resolve<IDaoTypeDocument>();
        private TakeDocDataAccess.Parameter.Interface.IDaoEntity daoEntity = UnityHelper.Resolve<TakeDocDataAccess.Parameter.Interface.IDaoEntity>();
        private TakeDocDataAccess.Security.Interface.IDaoUserTk daoUser = UnityHelper.Resolve<TakeDocDataAccess.Security.Interface.IDaoUserTk>(); 

        public ICollection<TakeDocModel.TypeDocument> Get(Guid userId, Guid entityId)
        {
            ICollection<TakeDocModel.TypeDocument> typeDocuments = daoTypeDocument.GetBy(x => x.EntityId == entityId);
            return typeDocuments;
        }

        public void AddDataField(string typeDocumentRef, string dataFieldRef, bool mandatory, bool delete, int? index, string entityRef, string userRef)
        {
            this.context.AddFieldToDocumentType(typeDocumentRef, dataFieldRef, mandatory, delete, index, entityRef, userRef);
        }

        public void AddDataField(Guid typeDocumentId, string dataFieldRef, bool mandatory, bool delete, int? index, Guid entityId, Guid userId)
        {
            string userRef = daoUser.GetBy(x => x.UserTkId == userId).First().UserTkReference;
            string entityRef = daoEntity.GetBy(x => x.EntityId == entityId).First().EntityReference;
            string typeDocumentRef = daoTypeDocument.GetBy(x => x.TypeDocumentId == typeDocumentId).First().TypeDocumentReference;

            this.context.AddFieldToDocumentType(typeDocumentRef, dataFieldRef, mandatory, delete, index, entityRef, userRef);
        }

        public void Update(TakeDocModel.TypeDocument typeDocument, Guid userId)
        {
            typeDocument.DateUpdateData = System.DateTime.UtcNow;
            typeDocument.UserUpdateData = userId;
            daoTypeDocument.Update(typeDocument);
        }

        public ICollection<TakeDocModel.TypeDocument> GetBy(Expression<Func<TakeDocModel.TypeDocument, bool>> where, params Expression<Func<TakeDocModel.TypeDocument, object>>[] properties)
        {
            return daoTypeDocument.GetBy(where, properties);
        }
    }
}
