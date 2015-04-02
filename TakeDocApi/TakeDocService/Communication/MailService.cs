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

        public void Send(string subject, string body, string to)
        {
            string login = daoParameter.GetBy(x => x.ParameterReference == "MAIL_SMTP_LOGIN").First().ParameterValue;
            string password = daoParameter.GetBy(x => x.ParameterReference == "MAIL_SMTP_PASSWORD").First().ParameterValue;
            string smtp = daoParameter.GetBy(x => x.ParameterReference == "MAIL_SERVER_SMTP").First().ParameterValue;
            string from = daoParameter.GetBy(x => x.ParameterReference == "MAIL_REQUEST_ACCOUNT_FROM").First().ParameterValue;
            string port = daoParameter.GetBy(x => x.ParameterReference == "MAIL_SMTP_PORT").First().ParameterValue;

            System.Net.NetworkCredential basicCredential =  new System.Net.NetworkCredential(login, password);
            MailMessage mail = new MailMessage(from, to);
            SmtpClient client = new SmtpClient();
            client.Port = Convert.ToInt32(port);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = basicCredential;
            client.EnableSsl = true;
            client.Host = smtp;
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            client.Send(mail);
        }
    }
}
