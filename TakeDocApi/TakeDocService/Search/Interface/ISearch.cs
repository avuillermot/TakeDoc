using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocService.Search.Interface
{
    public interface ISearch
    {
        ICollection<TakeDocModel.SearchUserTk_Result> SearchUser(string firstName, string lastName, string email, Guid entityId);
    }
}
