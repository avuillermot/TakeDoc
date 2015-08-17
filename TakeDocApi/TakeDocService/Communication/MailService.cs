using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace TakeDocService.Communication
{
    public class MailService : Interface.IMailService
    {
        TakeDocDataAccess.DaoBase<TakeDocModel.Parameter> daoParameter = new TakeDocDataAccess.DaoBase<TakeDocModel.Parameter>();
        TakeDocService.Security.Interface.ICryptoService servCrypto = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocService.Security.Interface.ICryptoService>();

        public void Send(string subject, string body, string from, string to, TakeDocModel.UserTk user)
        {
            TakeDocDataAccess.DaoBase<TakeDocModel.Parameter> daoParameter = new TakeDocDataAccess.DaoBase<TakeDocModel.Parameter>();

            string overloadTo = daoParameter.GetBy(x => x.ParameterReference == "REDIRECT_MAIL").First().ParameterValue;
            if (string.IsNullOrEmpty(overloadTo) == false) to = overloadTo;

            string login = daoParameter.GetBy(x => x.ParameterReference == "MAIL_SMTP_LOGIN").First().ParameterValue;
            string password = daoParameter.GetBy(x => x.ParameterReference == "MAIL_SMTP_PASSWORD").First().ParameterValue;
            string smtp = daoParameter.GetBy(x => x.ParameterReference == "MAIL_SERVER_SMTP").First().ParameterValue;
            string port = daoParameter.GetBy(x => x.ParameterReference == "MAIL_SMTP_PORT").First().ParameterValue;

            System.Net.NetworkCredential basicCredential =  new System.Net.NetworkCredential(login, password);
            MailMessage mail = new MailMessage(from, to);
            SmtpClient client = new SmtpClient();
            client.Host = smtp;
            client.Port = Convert.ToInt32(port);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = basicCredential;
            client.EnableSsl = false;
            mail.Subject = subject;
            mail.Body = this.FillField(body, user);
            mail.IsBodyHtml = true;
            if (daoParameter.GetBy(x => x.ParameterReference == "SEND_MAIL").First().ParameterValue.ToUpper().Equals("TRUE"))
                client.Send(mail);
            else System.IO.File.WriteAllText(string.Concat(TakeDocModel.Environnement.TempDirectory, "mail",System.Guid.NewGuid(),".html"), mail.Body);
        }

        private string FillField(string body, TakeDocModel.UserTk user)
        {
            string back = body;
            back = back.Replace("{{Reference}}", user.UserTkReference);
            back = back.Replace("{{FirstName}}", user.UserTkFirstName);
            back = back.Replace("{{LastName}}", user.UserTkLastName);
            back = back.Replace("{{Password}}", servCrypto.Decrypt(user.UserTkPassword));
            back = back.Replace("{{UrlBase}}", this.GetFieldValue("URL_BASE"));
            return back;
        }

        private string GetFieldValue(string paramRef)
        {
            ICollection<TakeDocModel.Parameter> parameters = daoParameter.GetBy(x => x.ParameterReference == paramRef);
            if (parameters.Count != 1) return string.Empty;
            else return parameters.First().ParameterValue;
        }
    }
}
