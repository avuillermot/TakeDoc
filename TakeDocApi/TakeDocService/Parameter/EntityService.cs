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
        TakeDocDataAccess.Security.Interface.IDaoUserTk daoUser = UnityHelper.Resolve<TakeDocDataAccess.Security.Interface.IDaoUserTk>();

        public void AddUser(TakeDocModel.UserTk user, TakeDocModel.Entity entity)
        {
            daoEntity.AddUser(user, entity);
        }

        public void AddUser(Guid userId, Guid entityId)
        {
            TakeDocModel.Entity entity = daoEntity.GetBy(x => x.EntityId == entityId).First();
            TakeDocModel.UserTk user = daoUser.GetBy(x => x.UserTkId == userId).First();

            this.AddUser(user, entity);
        }
        public void RemoveUser(Guid userId, Guid entityId)
        {
            TakeDocModel.Entity entity = daoEntity.GetBy(x => x.EntityId == entityId).First();
            TakeDocModel.UserTk user = daoUser.GetBy(x => x.UserTkId == userId).First();

            daoEntity.RemoveUser(user, entity);
        }
    }
}
