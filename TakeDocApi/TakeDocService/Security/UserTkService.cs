using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace TakeDocService.Security
{
    public class UserTkService : BaseService, Interface.IUserTkService
    {
        TakeDocDataAccess.Security.Interface.IDaoUserTk daoUserTk = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocDataAccess.Security.Interface.IDaoUserTk>();
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
            this.Logger.InfoFormat("Log on {0}", login);
            TakeDocModel.UserTk user = this.GetByLogin(login);
            if (user != null && (user.UserTkPassword != password || user.UserTkEnable == false)) user = null;
            if (user == null) {
                string msg = string.Format("Utilisateur inconnu ou non identifié : {0}", login);
                this.Logger.Info(msg);
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
                claims.Add(new Claim(ue.EntityId.ToString(),ue.EntityLabel,"entityId"));
                claims.Add(new Claim(ue.EntityReference, ue.EntityLabel, "entityReference"));
            }

            ClaimsIdentity ci = new ClaimsIdentity(claims);
            
            return new ClaimsPrincipal(ci);
        }

        public TakeDocModel.UserTk Create(TakeDocModel.UserTk user, TakeDocModel.Entity entity) {
            this.Create(user);
            daoUserTk.AddEntity(user, entity);
            return user;
        }

        private TakeDocModel.UserTk Create(TakeDocModel.UserTk user)
        {
            bool emailExist = (daoUserTk.GetBy(x => x.UserTkEmail == user.UserTkEmail).Count() > 0);
            bool validPassword = user.UserTkPassword.Length >= 5;

            if (emailExist == true) base.CreateError("Email already exists");
            if (validPassword == false) base.CreateError("Invalid password");


            // encrypt password
            // check valid email
            bool isEmail = Regex.IsMatch(user.UserTkEmail, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
            if (isEmail == false)
            {
                string msg = string.Concat("Create User : invalid email {0}", user.UserTkEmail);
                this.Logger.Error(msg);
                throw new Exception(msg);
            }

            // format name
            user.UserTkLastName = user.UserTkLastName.ToUpper();
            if (user.UserTkFirstName.Length >= 2) user.UserTkFirstName = user.UserTkFirstName.Substring(0, 1).ToUpper() + user.UserTkFirstName.Substring(1).ToLower();
            else user.UserTkFirstName = user.UserTkFirstName.ToUpper();

            user.UserTkEnable = false;

            try
            {
                return daoUserTk.Create(user);
            }
            catch (Exception ex)
            {
                string msg = string.Format("Error creation user name : {0} {1} - email : {2} - password : {3}", user.UserTkFirstName, user.UserTkLastName, user.UserTkEmail, user.UserTkPassword);
                this.Logger.Error(ex);
                base.CreateError(msg);
                return null;
            }
        }

        public bool ActivateUser(string userRef)
        {
            bool back = false;
            ICollection<TakeDocModel.UserTk> users = daoUserTk.GetBy(x => x.UserTkReference == userRef);
            if (users.Count == 0) base.CreateError(string.Format("no user with ref {0}"));
            if (users.Count > 1) base.CreateError(string.Format("to many user with ref {0}", userRef));

            TakeDocModel.UserTk user = users.First();
            if (user.UserTkActivate == true) base.CreateError(string.Format("user {0} already activate",user.UserTkLogin));

            user.UserTkActivate = true;
            daoUserTk.Update(user);

            return back;
        }

    }
}
