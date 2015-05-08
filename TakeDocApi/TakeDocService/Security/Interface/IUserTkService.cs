using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;

namespace TakeDocService.Security.Interface
{
    public interface IUserTkService
    {
        ICollection<TakeDocModel.UserTk> GetBy(Expression<Func<TakeDocModel.UserTk, bool>> where, params Expression<Func<TakeDocModel.UserTk, object>>[] properties);
        TakeDocModel.UserTk GetByLogin(string login);
        ClaimsPrincipal GetClaimsByLogin(string login);
        ICollection<TakeDocModel.UserTk> GetAll();
        TakeDocModel.UserTk Logon(string login, string password);
        ClaimsPrincipal GetClaimsPrincipal(TakeDocModel.UserTk user);
        TakeDocModel.UserTk Create(TakeDocModel.UserTk user, TakeDocModel.Entity entity);
        void Update(TakeDocModel.UserTk user);
        void ChangePassword(Guid userId, string olderPaswword, string newPassword);
        string GenerateNewPassword(Guid userId);
        void Delete(Guid userId, Guid currentUserId);
        ICollection<TakeDocModel.UserTk> GetByName(Guid userId, string name);
    }
}
