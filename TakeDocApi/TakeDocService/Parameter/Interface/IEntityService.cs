using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocService.Parameter.Interface
{
    public interface IEntityService
    {
        void AddUser(TakeDocModel.UserTk user, TakeDocModel.Entity entity, bool isAccountCreation);
        void RemoveUser(TakeDocModel.UserTk user, TakeDocModel.Entity entity);
        void AddUser(Guid userId, Guid entityId, bool isAccountCreation);
        void RemoveUser(Guid userId, Guid entityId);
    }
}
