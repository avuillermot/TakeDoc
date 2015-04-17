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
            bool back = servUser.ActivateUser(userRef);
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
                    this.SendMail(user, entitys.First());

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

        private void SendMail(TakeDocModel.UserTk user, TakeDocModel.Entity entity)
        {
            string title = daoParameter.GetBy(x => x.ParameterReference == "MAIL_ACTIVATE_ACCOUNT_TITLE").First().ParameterValue;
            string bodyFile = daoParameter.GetBy(x => x.ParameterReference == "MAIL_ACTIVATE_ACCOUNT_BODY").First().ParameterValue;

            string path = string.Concat(TakeDocModel.Environnement.ModelDirectory, entity.EntityReference, @"\", "mail", @"\", bodyFile);
            string body = System.IO.File.ReadAllText(path);

            servMail.Send(title, body, user.UserTkEmail, user);
        }
    }
}
