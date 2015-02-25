using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocService.Security.Interface
{
    public interface IUserTkService
    {
        TakeDocModel.UserTk GetByLogin(string login);
        ICollection<TakeDocModel.UserTk> GetAll();
    }
}
