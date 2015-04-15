using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.MyUnityHelper;

namespace TakeDocService.Parameter
{
    public class EntityService : Interface.IEntityService
    {
        TakeDocDataAccess.Parameter.Interface.IDaoEntity daoEntity = UnityHelper.Resolve<TakeDocDataAccess.Parameter.Interface.IDaoEntity>();

        public void AddUser(TakeDocModel.UserTk user, TakeDocModel.Entity entity)
        {
            daoEntity.AddUser(user, entity);
        }

        public void RemoveUser(TakeDocModel.UserTk user, TakeDocModel.Entity entity)
        {
            daoEntity.RemoveUser(user, entity);
        }
    }
}
