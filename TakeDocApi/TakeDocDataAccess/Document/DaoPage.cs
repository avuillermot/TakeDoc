using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocDataAccess.Document
{
    public class DaoPage : DaoBase<TakeDocModel.Page>, Interface.IDaoPage
    {
        public TakeDocModel.Page Add(Guid userId, Guid entityId, Guid versionId)
        {
            TakeDocModel.Page page = new TakeDocModel.Page();

            page.PageId = System.Guid.NewGuid();
            page.EntityId = entityId;
            page.PageReference = base.Context.GenerateReference("Page");
            page.PageVersionId = versionId;

            page.DateCreateData = System.DateTimeOffset.UtcNow;
            page.UserCreateData = userId;
            page.EtatDeleteData = false;
            page.PageNumber = this.GetBy(x => x.PageVersionId == versionId).Count() + 1;

            base.Context.Page.Add(page);
            ctx.SaveChanges();
            return page;
        }
    }
}
