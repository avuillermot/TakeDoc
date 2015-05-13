using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocService.Search.Interface
{
    public interface ISearch
    {
        /// <summary>
        /// Search a user
        /// </summary>
        /// <param name="currentUserId">user who do seach, only entity allow for this user are use in search</param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="email"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        ICollection<TakeDocModel.SearchUserTk_Result> SearchUser(Guid currentUserId, string firstName, string lastName, string email, Guid entityId);
        /// <summary>
        /// Search user who contains value parameter in first name, last name or email
        /// </summary>
        /// <param name="currentUserId">user who do seach, only entity allow for this user are use in search</param>
        /// <param name="value"></param>
        /// <returns></returns>
        ICollection<TakeDocModel.SearchUserTkFullText_Result> SearchUserFullText(Guid currentUserId, string value, Guid entityId);
    }
}
