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
    public class TypeDocumentsController : oDataBase<TypeDocument>
    {
        private static ODataValidationSettings _validationSettings = new ODataValidationSettings();

        // GET: odata/TypeDocuments
        //[Authorize]
        public IHttpActionResult GetTypeDocuments(ODataQueryOptions<TypeDocument> queryOptions)
        {
            ICollection<TypeDocument> items = null;
            try
            {
                items = this.Get(queryOptions, _validationSettings);
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok<IEnumerable<TypeDocument>>(items);
        }
    }
}
