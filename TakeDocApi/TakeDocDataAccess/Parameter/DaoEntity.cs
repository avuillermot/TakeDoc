using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocDataAccess.Parameter
{
    public class DaoEntity : DaoBase<TakeDocModel.Entity>, Interface.IDaoEntity
    {
        public void AddUser(TakeDocModel.UserTk user, TakeDocModel.Entity entity)
        {
            base.Context.AddUserToEntity(user.UserTkReference, entity.EntityReference);
        }

        public void RemoveUser(TakeDocModel.UserTk user, TakeDocModel.Entity entity)
        {
            base.Context.DeleteUserToEntity(user.UserTkReference, entity.EntityReference);
        }
    }
}
