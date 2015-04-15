using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocDataAccess.Security
{
    public class DaoUserTk :  DaoBase<TakeDocModel.UserTk>, Interface.IDaoUserTk
    {
        public TakeDocModel.UserTk Create(TakeDocModel.UserTk user)
        {
            string reference = this.Context.GenerateReference("UserTk");
            user.UserTkReference = reference;
            user.UserTkId = Guid.NewGuid();
            user.UserTkDateCreateData = System.DateTimeOffset.UtcNow;

            base.Context.UserTk.Add(user);
            base.Context.SaveChanges();

            return user;
        }

        public new void Delete(TakeDocModel.UserTk user)
        {
            this.Context.DeleteEntityUser(user.UserTkReference);
            base.Delete(user);
        }

        public new void Delete(ICollection<TakeDocModel.UserTk> users)
        {
            foreach(TakeDocModel.UserTk user in users) this.Context.DeleteEntityUser(user.UserTkReference);
            base.Delete(users);
        }
    }
}
