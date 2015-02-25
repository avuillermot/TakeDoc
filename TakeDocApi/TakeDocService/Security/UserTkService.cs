using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocService.Security
{
    public class UserTkService : BaseService, Interface.IUserTkService
    {
        TakeDocDataAccess.DaoBase<TakeDocModel.UserTk> daoUserTk = new TakeDocDataAccess.DaoBase<TakeDocModel.UserTk>();
        TakeDocDataAccess.DaoBase<TakeDocModel.View_UserEntity> daoViewUserEntity = new TakeDocDataAccess.DaoBase<TakeDocModel.View_UserEntity>();

        public TakeDocModel.UserTk GetByLogin(string login)
        {
            ICollection<TakeDocModel.UserTk> users = daoUserTk.GetBy(x => x.UserTkLogin == login && x.UserTkExternalAccount == false);
            if (users.Count() > 1) base.Logger.Error(string.Format("Le login {0} ne peut pas exister plusileurs fois.", login));
            else if (users.Count() == 0) return null;
            TakeDocModel.UserTk user = users.First();

            user.Entitys = daoViewUserEntity.GetBy(x => x.UserTkId == user.UserTkId);

            return user;
        }

        public ICollection<TakeDocModel.UserTk> GetAll()
        {
            ICollection<TakeDocModel.UserTk> users = daoUserTk.GetAll();
            ICollection<TakeDocModel.View_UserEntity> userEntitys = daoViewUserEntity.GetAll();

            foreach (TakeDocModel.UserTk user in users.Where(x => x.UserTkExternalAccount == false))
            {
                user.Entitys = new List<TakeDocModel.View_UserEntity>();
                ICollection<TakeDocModel.View_UserEntity> ues = userEntitys.Where(x => x.UserTkId == user.UserTkId).ToList();
                foreach (TakeDocModel.View_UserEntity ue in ues) user.Entitys.Add(ue);
            }

            return users;
        }
    }
}
