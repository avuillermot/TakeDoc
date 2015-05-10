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
        private TakeDocDataAccess.DaoBase<TakeDocModel.Type_Validation> daoTypeValidation = new TakeDocDataAccess.DaoBase<TakeDocModel.Type_Validation>();
        private TakeDocDataAccess.DaoBase<TakeDocModel.BackOfficeTypeDocument> daoBackofficeUser = new TakeDocDataAccess.DaoBase<TakeDocModel.BackOfficeTypeDocument>();

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

        public void Delete(Guid typeDocumentId, Guid userId, Guid entityId)
        {
            TakeDocModel.TypeDocument current = daoTypeDocument.GetBy(x => x.TypeDocumentId == typeDocumentId && x.EntityId == entityId).First();
            current.EtatDeleteData = true;
            current.DateDeleteData = System.DateTimeOffset.UtcNow;
            current.UserDeleteData = userId;
            daoTypeDocument.Update(current);
        }

        public ICollection<TakeDocModel.TypeDocument> GetBy(Expression<Func<TakeDocModel.TypeDocument, bool>> where, params Expression<Func<TakeDocModel.TypeDocument, object>>[] properties)
        {
            return daoTypeDocument.GetBy(where, properties);
        }

        public TakeDocModel.TypeDocument Add(string label, Guid entityId, Guid userId)
        {
            string normalized = label.Normalize(NormalizationForm.FormD);
            StringBuilder resultBuilder = new StringBuilder();
            foreach (var character in normalized)
            {
                System.Globalization.UnicodeCategory category = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(character);
                if (category == System.Globalization.UnicodeCategory.LowercaseLetter
                    || category == System.Globalization.UnicodeCategory.UppercaseLetter
                    || category == System.Globalization.UnicodeCategory.SpaceSeparator)
                    resultBuilder.Append(character);
            }
            string reference = System.Text.RegularExpressions.Regex.Replace(resultBuilder.ToString(), @"\s+", "-");

            TakeDocModel.TypeDocument toAdd = new TakeDocModel.TypeDocument();
            toAdd.TypeDocumentId = System.Guid.NewGuid();
            toAdd.TypeDocumentLabel = label;
            toAdd.TypeDocumentReference = reference.ToUpper();
            toAdd.TypeDocumentPageNeed = false;
            toAdd.TypeDocumentValidationId = daoTypeValidation.GetBy(x => x.TypeValidationReference == "NO").First().TypeValidationId;
            toAdd.UserCreateData = userId;
            toAdd.DateCreateData = System.DateTimeOffset.UtcNow;
            toAdd.EntityId = entityId;
            toAdd.EtatDeleteData = false;
            return daoTypeDocument.Add(toAdd);
        }

        public void AddBackofficeUser(Guid userId, Guid typeDocumentId, Guid entityId)
        {
            ICollection<TakeDocModel.BackOfficeTypeDocument> bos = daoBackofficeUser.GetBy(x => x.UserTkId == userId && x.TypeDocumentId == typeDocumentId && x.EntityId == entityId);
            TakeDocModel.BackOfficeTypeDocument bo = null;
            if (bos.Count() > 0)
            {
                bo = bos.First();
                bo.UserUpdateData = userId;
                bo.DateUpdateData = System.DateTimeOffset.UtcNow;
                if (bo.EtatDeleteData) bo.EtatDeleteData = false;
                daoBackofficeUser.Update(bo);
            }
            else
            {
                bo = new TakeDocModel.BackOfficeTypeDocument();
                bo.UserCreateData = userId;
                bo.DateCreateData = System.DateTimeOffset.UtcNow;
                daoBackofficeUser.Context.BackOfficeTypeDocument.Add(bo);
            }
            daoBackofficeUser.Context.SaveChanges();
        }

        public void DeleteBackofficeUser(Guid userId, Guid typeDocumentId, Guid entityId)
        {

        }
    }
}
