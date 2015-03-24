using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocService.External
{
    public class CustomerService : TakeDocService.BaseService, Interface.ICustomerService
    {
        private TakeDocDataAccess.External.Interface.IDaoCustomer daoCustomer = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocDataAccess.External.Interface.IDaoCustomer>();
        public ICollection<TakeDocModel.Customer> NameContains(string value, Guid entityId)
        {
            return daoCustomer.GetBy(x => x.CustomerName.IndexOf(value) > -1 && x.EntityId == entityId);
        }
    }
}
