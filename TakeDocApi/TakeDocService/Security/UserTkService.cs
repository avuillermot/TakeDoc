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
        TakeDocDataAccess.DaoBase<TakeDocModel.Document> daoDocument = new TakeDocDataAccess.DaoBase<TakeDocModel.Document>();

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

        public ICollection<TakeDocModel.Dto.Stats.Dashboard> GetDashboard(Guid userId)
        {
            ICollection<TakeDocModel.Dto.Stats.Dashboard> back = new List<TakeDocModel.Dto.Stats.Dashboard>();
            ICollection<TakeDocModel.View_UserEntity> userEntitys = daoViewUserEntity.GetBy(x => x.UserTkId == userId);
            foreach (TakeDocModel.View_UserEntity vue in userEntitys.Where(x => x.EtatDeleteData == false))
            {
                ICollection<TakeDocModel.Document> documents = daoDocument.GetBy(x =>
                    (x.Status_Document.StatusDocumentReference == TakeDocModel.Status_Document.Create
                    || x.Status_Document.StatusDocumentReference == TakeDocModel.Status_Document.Incomplete
                    || x.Status_Document.StatusDocumentReference == TakeDocModel.Status_Document.Complete
                    || x.Status_Document.StatusDocumentReference == TakeDocModel.Status_Document.Send)
                    && x.EntityId == vue.EntityId && x.DocumentOwner == vue.UserTkId).ToList();

                int nbCreate = documents.Where(x => x.Status_Document.StatusDocumentReference == TakeDocModel.Status_Document.Create).Count();
                int nbIncomplete = documents.Where(x => x.Status_Document.StatusDocumentReference == TakeDocModel.Status_Document.Incomplete).Count();
                int nbComplete = documents.Where(x => x.Status_Document.StatusDocumentReference == TakeDocModel.Status_Document.Complete).Count();
                int nbSend = documents.Where(x => x.Status_Document.StatusDocumentReference == TakeDocModel.Status_Document.Send).Count();

                back.Add(new TakeDocModel.Dto.Stats.Dashboard()
                {
                    EntityId = vue.EntityId,
                    Type = "STATUS_DOCUMENT",
                    Code = TakeDocModel.Status_Document.Create,
                    Value = nbCreate
                });
                back.Add(new TakeDocModel.Dto.Stats.Dashboard()
                {
                    EntityId = vue.EntityId,
                    Type = "STATUS_DOCUMENT",
                    Code = TakeDocModel.Status_Document.Incomplete,
                    Value = nbIncomplete
                });
                back.Add(new TakeDocModel.Dto.Stats.Dashboard()
                {
                    EntityId = vue.EntityId,
                    Type = "STATUS_DOCUMENT",
                    Code = TakeDocModel.Status_Document.Complete,
                    Value = nbComplete
                });
                back.Add(new TakeDocModel.Dto.Stats.Dashboard()
                {
                    EntityId = vue.EntityId,
                    Type = "STATUS_DOCUMENT",
                    Code = TakeDocModel.Status_Document.Send,
                    Value = nbSend
                });
            }
            return back;
        }
    }
}
