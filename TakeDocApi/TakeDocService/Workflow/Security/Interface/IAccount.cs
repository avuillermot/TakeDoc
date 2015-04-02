using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocService.Workflow.Security.Interface
{
    /// <summary>
    /// A user ask a account, the request will be create on database
    /// </summary>
    public interface IAccount
    {
        bool CreateRequest(string firstName, string lastName, string email, string password, string culture, string entityRef);
        bool ActivateUser(string userRef);
    }
}
