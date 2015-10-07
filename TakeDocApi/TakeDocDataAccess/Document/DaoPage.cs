using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocDataAccess.Document
{
    public class DaoPage : DaoBase<TakeDocModel.Page>, Interface.IDaoPage
    {
        public TakeDocModel.Page Add(Guid userId, Guid entityId, Guid versionId, string extension, int rotation, int pageNumber)
        {
            TakeDocModel.Page page = new TakeDocModel.Page();

            page.PageId = System.Guid.NewGuid();
            page.EntityId = entityId;
            page.PageReference = base.Context.GenerateReference("Page");
            page.PageVersionId = versionId;

            page.DateCreateData = System.DateTimeOffset.UtcNow;
            page.UserCreateData = userId;
            page.EtatDeleteData = false;
            page.PageNumber = (pageNumber != Int32.MinValue) ? pageNumber : this.GetBy(x => x.PageVersionId == versionId).Count() + 1;
            page.PageRotation = rotation;
            page.PageFileExtension = extension.ToLower();

            base.Context.Page.Add(page);
            ctx.SaveChanges();
            return page;
        }
    }
}
