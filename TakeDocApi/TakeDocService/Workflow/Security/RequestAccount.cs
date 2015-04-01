using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocService.Workflow.Security
{
    public class RequestAccount : Interface.IRequestAccount
    {
        TakeDocService.Security.Interface.IUserTkService servUser = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocService.Security.Interface.IUserTkService>();

        public bool Execute(string firstName, string lastName, string email, string password, string culture) {
            bool back = false;

            TakeDocModel.UserTk user = new TakeDocModel.UserTk()
            {
                UserTkFirstName = firstName,
                UserTkLastName = lastName,
                UserTkEmail = email,
                UserTkLogin = email,
                UserTkPassword = password,
                UserTkCulture = culture,
                UserTkExterneId = string.Empty,
            };
            servUser.Create(user);

            // send mail to administrator

            // send mail to user

            return back;
        }
    }
}
