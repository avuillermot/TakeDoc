using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocDataAccess.Document
{
    public class DaoBackOfficeTypeDocument : DaoBase<TakeDocModel.BackOfficeTypeDocument>, Interface.IDaoBackOfficeTypeDocument
    {
        public void Add(Guid userIdToAdd, Guid typeDocumentId, Guid entityId, Guid userIdUpdater)
        {
            ICollection<TakeDocModel.BackOfficeTypeDocument> bos = this.GetBy(x => x.UserTkId == userIdToAdd && x.TypeDocumentId == typeDocumentId && x.EntityId == entityId);
            TakeDocModel.BackOfficeTypeDocument bo = null;
            if (bos.Count() > 0)
            {
                bo = bos.First();
                bo.UserUpdateData = userIdUpdater;
                bo.DateUpdateData = System.DateTimeOffset.UtcNow;
                if (bo.EtatDeleteData) bo.EtatDeleteData = false;
                this.Update(bo);
            }
            else
            {
                bo = new TakeDocModel.BackOfficeTypeDocument();
                bo.UserTkId = userIdToAdd;
                bo.EntityId = entityId;
                bo.TypeDocumentId = typeDocumentId;
                bo.UserCreateData = userIdUpdater;
                bo.DateCreateData = System.DateTimeOffset.UtcNow;
                bo.EtatDeleteData = false;
                this.Context.BackOfficeTypeDocument.Add(bo);
            }
            this.Context.SaveChanges();
        }

        public void Delete(Guid userIdToDel, Guid typeDocumentId, Guid entityId, Guid userIdUpdater)
        {
            ICollection<TakeDocModel.BackOfficeTypeDocument> bos = this.GetBy(x => x.UserTkId == userIdToDel && x.TypeDocumentId == typeDocumentId && x.EntityId == entityId);
            TakeDocModel.BackOfficeTypeDocument bo = null;
            if (bos.Count() > 0)
            {
                bo = bos.First();
                bo.UserDeleteData = userIdUpdater;
                bo.DateDeleteData = System.DateTimeOffset.UtcNow;
                bo.EtatDeleteData = true;
                this.Update(bo);
            }
        }

        public new ICollection<TakeDocModel.BackOfficeTypeDocument> GetBy(Expression<Func<TakeDocModel.BackOfficeTypeDocument, bool>> where, params Expression<Func<TakeDocModel.BackOfficeTypeDocument, object>>[] properties)
        {
            return base.GetBy(where, properties);
        }
    }
}
