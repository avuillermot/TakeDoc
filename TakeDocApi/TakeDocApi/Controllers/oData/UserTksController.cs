using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData.Query;
using System.Web.Http.OData.Routing;
using TakeDocModel;
using Microsoft.Data.OData;



namespace TakeDocApi.Controllers.oData
{
    public class UserTksController : oDataBase<UserTk>
    {
        private static ODataValidationSettings _validationSettings = new ODataValidationSettings();

        // GET: odata/UserTks
        public IHttpActionResult GetUserTks(ODataQueryOptions<UserTk> queryOptions)
        {
            ICollection<UserTk> items = null;
            try
            {
                items = this.Get(queryOptions, _validationSettings);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok<IEnumerable<UserTk>>(items);
        }
    }
}
