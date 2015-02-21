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
        public IHttpActionResult GetTypeDocuments(ODataQueryOptions<TypeDocument> queryOptions)
        {
            ICollection<TypeDocument> items = this.Get(queryOptions, _validationSettings);
            return Ok<IEnumerable<TypeDocument>>(items);
        }

        // GET: odata/TypeDocuments(5)
        public IHttpActionResult GetTypeDocument([FromODataUri] System.Guid key, ODataQueryOptions<TypeDocument> queryOptions)
        {
            // validate the query.
            try
            {
                queryOptions.Validate(_validationSettings);
            }
            catch (ODataException ex)
            {
                return BadRequest(ex.Message);
            }

            // return Ok<TypeDocument>(typeDocument);
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        // PUT: odata/TypeDocuments(5)
        public IHttpActionResult Put([FromODataUri] System.Guid key, Delta<TypeDocument> delta)
        {
            Validate(delta.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Get the entity here.

            // delta.Put(typeDocument);

            // TODO: Save the patched entity.

            // return Updated(typeDocument);
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        // POST: odata/TypeDocuments
        public IHttpActionResult Post(TypeDocument typeDocument)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Add create logic here.

            // return Created(typeDocument);
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        // PATCH: odata/TypeDocuments(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] System.Guid key, Delta<TypeDocument> delta)
        {
            Validate(delta.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Get the entity here.

            // delta.Patch(typeDocument);

            // TODO: Save the patched entity.

            // return Updated(typeDocument);
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        // DELETE: odata/TypeDocuments(5)
        public IHttpActionResult Delete([FromODataUri] System.Guid key)
        {
            // TODO: Add delete logic here.

            // return StatusCode(HttpStatusCode.NoContent);
            return StatusCode(HttpStatusCode.NotImplemented);
        }
    }
}
