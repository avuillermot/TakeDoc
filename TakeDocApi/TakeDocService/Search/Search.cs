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

        public ICollection<SearchUserTk_Result> SearchUser(string firstName, string lastName, string email, Guid entityId)
        {
            ObjectResult<SearchUserTk_Result> back = context.SearchUserTk(firstName, lastName, email, entityId);
            return back.ToList();
        }
    }
}
