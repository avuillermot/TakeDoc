using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocDataAccess
{
    public class DaoBase<T> : DaoGeneric<T> where T : class
    {
        public DaoBase() {
            base.ctx = (System.Data.Entity.DbContext) Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocModel.TakeDocEntities1>();
        }

        public TakeDocModel.TakeDocEntities1 Context {
            get {
                return (TakeDocModel.TakeDocEntities1) base.ctx;
            }
        }

        public string GenerateReference(string entityName)
        {
            return this.Context.GenerateReference(entityName);
        }
    }
}
