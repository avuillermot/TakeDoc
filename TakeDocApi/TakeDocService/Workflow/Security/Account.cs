using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocService.Workflow.Security
{
    public class Account : BaseService, Interface.IAccount
    {
        TakeDocService.Security.Interface.IUserTkService servUser = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocService.Security.Interface.IUserTkService>();
        TakeDocService.Security.Interface.IGroupeTkService servGroupe = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocService.Security.Interface.IGroupeTkService>();
        TakeDocService.Communication.Interface.IMailService servMail = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocService.Communication.Interface.IMailService>();

        TakeDocDataAccess.DaoBase<TakeDocModel.Entity> daoEntity = new TakeDocDataAccess.DaoBase<TakeDocModel.Entity>();
        TakeDocDataAccess.DaoBase<TakeDocModel.Parameter> daoParameter = new TakeDocDataAccess.DaoBase<TakeDocModel.Parameter>();

        public bool ActivateUser(string userRef)
        {
            bool back = false;
            ICollection<TakeDocModel.UserTk> users = servUser.GetBy(x => x.UserTkReference == userRef);
            if (users.Count != 1) base.CreateError(string.Format("L'utilisateur {0} n'existe pas.", userRef));

            TakeDocModel.UserTk user = users.First();
            if (user.UserTkActivate == true) base.CreateError(string.Format("Utilisateur {0} déjà activer", user.UserTkLogin));

            user.UserTkActivate = true;
            servUser.Update(user);

            return back;
        }

        public bool CreateRequest(string firstName, string lastName, string email, string password, string culture, string entityRef) {
            bool back = false;

            using (System.Transactions.TransactionScope tr = new System.Transactions.TransactionScope())
            {

                ICollection<TakeDocModel.Entity> entitys = daoEntity.GetBy(x => x.EntityReference == entityRef);
                if (entitys.Count() == 0) base.CreateError("Entity unknow");
                Guid entityId = entitys.First().EntityId;
                TakeDocModel.UserTk user = new TakeDocModel.UserTk()
                {
                    UserTkFirstName = firstName,
                    UserTkLastName = lastName,
                    UserTkEmail = email,
                    UserTkLogin = email,
                    UserTkPassword = password,
                    UserTkCulture = culture,
                    UserTkExterneId = null,
                    UserTkActivate = false,
                    GroupTk = servGroupe.GetBy(x => x.GroupTkReference == "USER").First()
                };
                try
                {
                    servUser.Create(user, entitys.First());
                    //throw new Exception("send mail to admin in order to inform");

                    back = true;
                    tr.Complete();
                }
                catch (Exception ex)
                {
                    this.Logger.Error(ex);
                    back = false;
                    throw ex;
                }

            }
            return back;
        }

        
    }
}
