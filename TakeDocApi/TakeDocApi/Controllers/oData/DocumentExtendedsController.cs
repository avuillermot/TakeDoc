using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using System.Web.Http.OData.Routing;
using TakeDocModel;
using Microsoft.Data.OData;

namespace TakeDocApi.Controllers.oData
{
    public class DocumentExtendedsController : oDataBase<View_DocumentExtended>
    {
        private static ODataValidationSettings _validationSettings = new ODataValidationSettings();

        // GET: odata/DocumentExtendeds
        public IHttpActionResult GetDocumentsExtended(ODataQueryOptions<View_DocumentExtended> queryOptions)
        {
            ICollection<View_DocumentExtended> items = null;
            try
            {
                items = this.Get(queryOptions, _validationSettings);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok<IEnumerable<TakeDocModel.View_DocumentExtended>>(items);
        }
    }
}
