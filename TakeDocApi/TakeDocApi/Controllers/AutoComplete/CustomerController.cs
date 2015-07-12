using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TakeDocService.External.Interface;

namespace TakeDocApi.Controllers.AutoComplete
{
    [RoutePrefix("Customer")]
    public class CustomerController : ApiController
    {
        [HttpGet]
        [Route("{entityId}/{userId}/{value}")]
        [TakeDocApi.Controllers.Security.AuthorizeTk()]
        public HttpResponseMessage SetSend(Guid entityId, Guid userId, string value)
        {
            ICustomerService servCustomer = Utility.MyUnityHelper.UnityHelper.Resolve<ICustomerService>();
            try
            {
                ICollection<TakeDocModel.Customer> customers = servCustomer.NameContains(value, entityId);
                var req = from customer in customers
                          select new
                          {
                              key = customer.CustomerId,
                              text = customer.CustomerName
                          };
                return Request.CreateResponse(req.ToList());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
