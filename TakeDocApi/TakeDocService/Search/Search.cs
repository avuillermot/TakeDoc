using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Core.Objects;
using TakeDocModel;

namespace TakeDocService.Search
{
    public class Search : Interface.ISearch
    {
        TakeDocModel.TakeDocEntities1 context = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocModel.TakeDocEntities1>();

        public ICollection<SearchUserTk_Result> SearchUser(Guid currentUserId, string firstName, string lastName, string email, Guid entityId)
        {
            ObjectResult<SearchUserTk_Result> results = context.SearchUserTk(currentUserId, firstName, lastName, email, entityId);
            ICollection<SearchUserTk_Result> back = results.ToList();
            return back;
        }

        public ICollection<SearchUserTk_Result> SearchUser(Guid currentUserId, string firstName, string lastName, string email, Guid entityId, bool enable)
        {
            return this.SearchUser(currentUserId, firstName, lastName, email, entityId).Where(x => x.UserTkEnable == true).ToList();
        }

        public ICollection<TakeDocModel.SearchUserTkFullText_Result> SearchUserFullText(Guid currentUserId, string value, Guid entityId)
        {
            ObjectResult<TakeDocModel.SearchUserTkFullText_Result> results = context.SearchUserTkFullText(currentUserId, value, entityId);
            ICollection<TakeDocModel.SearchUserTkFullText_Result> back = results.ToList();
            return back;
        }

        public ICollection<TakeDocModel.SearchUserTkFullText_Result> SearchUserFullText(Guid currentUserId, string value, Guid entityId, bool enable)
        {
            return this.SearchUserFullText(currentUserId, value, entityId).Where(x => x.UserTkEnable == true).ToList();
        }
    }
}
