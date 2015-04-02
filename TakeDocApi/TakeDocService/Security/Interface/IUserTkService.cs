using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;

namespace TakeDocService.Security.Interface
{
    public interface IUserTkService
    {
        TakeDocModel.UserTk GetByLogin(string login);
        ClaimsPrincipal GetClaimsByLogin(string login);
        ICollection<TakeDocModel.UserTk> GetAll();
        TakeDocModel.UserTk Logon(string login, string password);
        ClaimsPrincipal GetClaimsPrincipal(TakeDocModel.UserTk user);
        TakeDocModel.UserTk Create(TakeDocModel.UserTk user, TakeDocModel.Entity entity);
        bool ActivateUser(string userRef);
    }
}
