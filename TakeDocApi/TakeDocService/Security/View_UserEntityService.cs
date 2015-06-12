using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using Utility.MyUnityHelper;

namespace TakeDocService.Security
{
    public class View_UserEntityService : BaseService, Interface.IView_UserEntityService
    {
        TakeDocDataAccess.DaoBase<TakeDocModel.View_UserEntity> daoEntityUser = UnityHelper.Resolve<TakeDocDataAccess.DaoBase<TakeDocModel.View_UserEntity>>();

        public ICollection<TakeDocModel.View_UserEntity> GetByUser(Guid userId)
        {
            return daoEntityUser.GetBy(x => x.UserTkId == userId && x.EtatDeleteData == false);
        }
    }
}
