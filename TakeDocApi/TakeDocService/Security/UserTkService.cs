using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;

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

        public ClaimsPrincipal GetClaimsByLogin(string login)
        {
            TakeDocModel.UserTk user = this.GetByLogin(login);
            return this.GetClaimsPrincipal(user);

        }

        public TakeDocModel.UserTk Logon(string login, string password)
        {
            TakeDocModel.UserTk user = this.GetByLogin(login);
            if (user != null && user.UserTkPassword != password) user = null;
            if (user == null) {
                string msg = "Utilisateur inconnu ou non authentifié.";
                base.Logger.Info(msg);
                throw new Exception(msg);
            }
            return user;
        }

        public ClaimsPrincipal GetClaimsPrincipal(TakeDocModel.UserTk user)
        {
            ICollection<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Email, user.UserTkEmail));
            claims.Add(new Claim(ClaimTypes.Name, user.UserTkLastName));
            claims.Add(new Claim(ClaimTypes.GivenName, user.UserTkFirstName));
            claims.Add(new Claim("ExternalAccount", user.UserTkExternalAccount.ToString()));
            claims.Add(new Claim("Reference", user.UserTkReference));
            claims.Add(new Claim("Login", user.UserTkLogin));
            claims.Add(new Claim("Id", user.UserTkId.ToString()));

            foreach (TakeDocModel.View_UserEntity ue in user.Entitys)
            {
                claims.Add(new Claim(ue.EntityId.ToString(),ue.EntityLibelle,"entityId"));
                claims.Add(new Claim(ue.EntityReference, ue.EntityLibelle, "entityReference"));
            }

            ClaimsIdentity ci = new ClaimsIdentity(claims);
            
            return new ClaimsPrincipal(ci);
        }
    }
}
