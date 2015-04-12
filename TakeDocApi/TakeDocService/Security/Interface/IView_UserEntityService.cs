using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocService.Security.Interface
{
    public interface IView_UserEntityService
    {
        ICollection<TakeDocModel.View_UserEntity> GetByUser(Guid userId);
    }
}
