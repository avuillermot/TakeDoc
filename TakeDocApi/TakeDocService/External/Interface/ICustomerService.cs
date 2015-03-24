using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocService.External.Interface
{
    public interface ICustomerService
    {
        ICollection<TakeDocModel.Customer> NameContains(string value, Guid entityId);
    }
}
