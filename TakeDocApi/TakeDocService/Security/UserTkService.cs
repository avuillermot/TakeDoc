﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        TakeDocDataAccess.DaoBase<TakeDocModel.GroupTk> daoGroupTk = new TakeDocDataAccess.DaoBase<TakeDocModel.GroupTk>();
        TakeDocDataAccess.DaoBase<TakeDocModel.Parameter> daoParameter = new TakeDocDataAccess.DaoBase<TakeDocModel.Parameter>();

        TakeDocService.Security.Interface.ICryptoService servCrypto = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocService.Security.Interface.ICryptoService>();
        TakeDocService.Security.Interface.ITokenService servToken = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocService.Security.Interface.ITokenService>();
        TakeDocService.Parameter.Interface.IEntityService servEntity = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocService.Parameter.Interface.IEntityService>();
        TakeDocService.Communication.Interface.IMailService servMail = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocService.Communication.Interface.IMailService>();

        TakeDocModel.TakeDocEntities1 context = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocModel.TakeDocEntities1>();

        public ICollection<TakeDocModel.UserTk> GetBy(Expression<Func<TakeDocModel.UserTk, bool>> where, params Expression<Func<TakeDocModel.UserTk, object>>[] properties)
        {
            ICollection<TakeDocModel.UserTk> users = daoUserTk.GetBy(where, properties);
            return users;
        }
        public TakeDocModel.UserTk GetByLogin(string login)
        {
            ICollection<TakeDocModel.UserTk> users = this.GetBy(x => x.UserTkLogin == login, x => x.GroupTk);
            if (users.Count() > 1) base.CreateError(string.Format("Le login {0} ne peut pas exister plusileurs fois.", login));
            else if (users.Count() == 0) base.CreateError(string.Format("Utilisateur {0} inconu.", login));
            TakeDocModel.UserTk user = users.First();

            return user;
        }

        public ICollection<TakeDocModel.UserTk> GetAll()
        {
            ICollection<TakeDocModel.UserTk> users = daoUserTk.GetAll();
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
            var passwordDecrypt = servCrypto.Decrypt(user.UserTkPassword);

            if (user != null && (password != passwordDecrypt || user.UserTkEnable == false || user.UserTkActivate == false)) user = null;
            if (user == null) {
                string msg = string.Format("Utilisateur inconnu ou non identifié : {0}", login);
                this.Logger.Info(msg);
                throw new Exception(msg);
            }

            TakeDocModel.RefreshToken token = servToken.CreateRefreshToken(user.UserTkId, "Logon");

            user.RefreshToken = token.Id;
            user.RefreshTokenTicks = token.DateEndUTC.Value.Ticks;
            user.AccessToken = token.AccessToken.First().Id;
            user.AccessTokenTicks = token.AccessToken.First().DateEndUTC.Value.Ticks;
            
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

            ClaimsIdentity ci = new ClaimsIdentity(claims);
            
            return new ClaimsPrincipal(ci);
        }

        public TakeDocModel.UserTk Create(TakeDocModel.UserTk user, TakeDocModel.Entity entity) {
            this.Create(user);
            servEntity.AddUser(user, entity);
            return user;
        }


        /// <summary>
        /// Check user data
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private bool Check(TakeDocModel.UserTk user, bool createMode)
        {
            bool emailExist = false;

            /*
             * createMode is true : verify if a user with this email exist,
             * createMode is false : verify if a user, not the current, with this email exist
            */
            if (createMode) emailExist = (this.GetBy(x => x.UserTkEmail == user.UserTkEmail).Count() > 0);
            else emailExist = (this.GetBy(x => x.UserTkEmail == user.UserTkEmail && x.UserTkId != user.UserTkId).Count() > 0);

            bool validPassword = servCrypto.Decrypt(user.UserTkPassword).Length >= 5;

            if (emailExist == true) base.CreateError("Cette adresse mail est déjà utilisée.");
            if (validPassword == false) base.CreateError("Votre mot de passe doit avoir au moins 5 caractères.");

            bool isEmail = Regex.IsMatch(user.UserTkEmail, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
            if (isEmail == false) base.CreateError(string.Format("Création d'un utilisateur : cette adresse mail n'est pas valide {0}", user.UserTkEmail));

            return true;
        }

        /// <summary>
        /// Format data in object
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private TakeDocModel.UserTk Format(TakeDocModel.UserTk user)
        {
            user.UserTkLastName = user.UserTkLastName.ToUpper();
            if (user.UserTkFirstName.Length >= 2) user.UserTkFirstName = user.UserTkFirstName.Substring(0, 1).ToUpper() + user.UserTkFirstName.Substring(1).ToLower();
            else user.UserTkFirstName = user.UserTkFirstName.ToUpper();
            return user;
        }

        private TakeDocModel.UserTk Create(TakeDocModel.UserTk user)
        {
            // encrypt password
            user.UserTkPassword = servCrypto.Encrypt(user.UserTkPassword);
            
            bool isValid = this.Check(user, true);
            user = this.Format(user);
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
        
        public void Update(TakeDocModel.UserTk user)
        {
            using (System.Transactions.TransactionScope tr = new System.Transactions.TransactionScope())
            {
                user.UserTkLogin = user.UserTkEmail;
                this.Check(user, false);
                user = this.Format(user);
                // we stored the first time when account is enable
                if (user.UserTkEnable == true && user.UserTkDateFirstEnable == null)
                {
                    user.UserTkDateFirstEnable = System.DateTime.UtcNow;
                    this.SendMailAccountEnable(user);
                }
                daoUserTk.Update(user);

                tr.Complete();
            }
        }

        private void SendMailAccountEnable(TakeDocModel.UserTk user)
        {

            string title = daoParameter.GetBy(x => x.ParameterReference == "MAIL_ACTIVATE_ACCOUNT_TITLE").First().ParameterValue;
            string bodyFile = daoParameter.GetBy(x => x.ParameterReference == "MAIL_ACTIVATE_ACCOUNT_BODY").First().ParameterValue;
            string from = daoParameter.GetBy(x => x.ParameterReference == "MAIL_ACTIVATE_ACCOUNT_FROM").First().ParameterValue;

            string path = string.Concat(TakeDocModel.Environnement.ModelDirectory, "MASTER", @"\", "mail", @"\", bodyFile);
            string body = System.IO.File.ReadAllText(path);

            servMail.Send(title, body, from, user.UserTkEmail, user);
        }

        public void ChangePassword(Guid userId, string olderPaswword, string newPassword)
        {
            using (System.Transactions.TransactionScope tr = new System.Transactions.TransactionScope())
            {
                ICollection<TakeDocModel.UserTk> users = this.GetBy(x => x.UserTkId == userId);
                if (users.Count != 1) base.CreateError("L'utilisateur est iconnu.");
                
                TakeDocModel.UserTk user = users.First();
                // check older password
                if (user.UserTkPassword != servCrypto.Encrypt(olderPaswword)) this.CreateError("Mot de passe erronné.");
                user.UserTkPassword = servCrypto.Encrypt(newPassword);
                this.Check(user, false);

                // update password
                daoUserTk.Update(user);

                tr.Complete();
            }
        }

        public string GenerateNewPassword(Guid userId)
        {
            string newPassWord = System.Web.Security.Membership.GeneratePassword(7, 0);

            ICollection<TakeDocModel.UserTk> users = this.GetBy(x => x.UserTkId == userId);
            if (users.Count != 1) base.CreateError("L'utilisateur est iconnu.");
            TakeDocModel.UserTk user = users.First();

            user.UserTkPassword = servCrypto.Encrypt(newPassWord);
            daoUserTk.Update(user);

            this.SendMailNewPassword(user);

            return newPassWord;
        }

        private void SendMailNewPassword(TakeDocModel.UserTk user)
        {
            string title = daoParameter.GetBy(x => x.ParameterReference == "MAIL_NEW_PASSWORD_TITLE").First().ParameterValue;
            string bodyFile = daoParameter.GetBy(x => x.ParameterReference == "MAIL_NEW_PASSWORD_BODY").First().ParameterValue;
            string from = daoParameter.GetBy(x => x.ParameterReference == "MAIL_NEW_PASSWORD_FROM").First().ParameterValue;

            string entityRef = "MASTER";
            string path = string.Concat(TakeDocModel.Environnement.ModelDirectory, entityRef, @"\", "mail", @"\", bodyFile);
            string body = System.IO.File.ReadAllText(path);

            servMail.Send(title, body, from, user.UserTkEmail, user);
        }

        public void Delete(Guid userId, Guid currentUserId)
        {
            using (System.Transactions.TransactionScope tr = new System.Transactions.TransactionScope())
            {
                TakeDocModel.UserTk user = daoUserTk.GetBy(x => x.UserTkId == userId).First();
                daoUserTk.Delete(user);
                context.AddEvent("REMOVE_USER", "UserTkService.Delete", string.Format("userId : {0} login : {1} by currentUserId {2}", userId, user.UserTkLogin, currentUserId));
                tr.Complete();
            }
        }

        public ICollection<TakeDocModel.UserTk> GetByName(Guid userId, string name)
        {
            TakeDocModel.UserTk user = daoUserTk.GetBy(x => x.UserTkId == userId).First();
            ICollection<TakeDocModel.UserTk> users = this.GetBy(x => x.UserTkLastName.StartsWith(name) || x.UserTkFirstName.StartsWith(name));
            return users;
        }
    }
}
