using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocService.Communication.Interface
{
    public interface IMailService
    {
        void Send(string subject, string body, string from, string to, TakeDocModel.UserTk user);
    }
}
