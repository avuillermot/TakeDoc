using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using System.Web.Http.OData.Routing;
using TakeDocModel;
using Microsoft.Data.OData;

namespace TakeDocApi.oData.Controllers
{
    public class DocumentsController : ODataController
    {
        private static ODataValidationSettings _validationSettings = new ODataValidationSettings();

        TakeDocEntities1 _db = new TakeDocEntities1();

        // GET: odata/Documents
        [Queryable]
        public async Task<IHttpActionResult> GetDocuments(ODataQueryOptions<Document> queryOptions)
        {
            // validate the query.
            ICollection<TakeDocModel.Document> documents;
            try
            {
                queryOptions.Validate(_validationSettings);
                IQueryable query = queryOptions.ApplyTo(_db.Document.AsQueryable());
                IQueryable<TakeDocModel.Document> data = query as IQueryable<TakeDocModel.Document>;
                documents = data.ToList();
            }
            catch (ODataException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok<IEnumerable<Document>>(documents);
            //return StatusCode(HttpStatusCode.NotImplemented);
        }

        // GET: odata/Documents(5)
        public async Task<IHttpActionResult> GetDocument([FromODataUri] System.Guid key, ODataQueryOptions<Document> queryOptions)
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

            // return Ok<Document>(document);
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        // PUT: odata/Documents(5)
        [Queryable]
        public async Task<IHttpActionResult> Put([FromODataUri] string key, Document document)
        {
           if (!ModelState.IsValid) return BadRequest(ModelState);

           try
           {
               TakeDocService.Document.Interface.IDocumentService servDoc = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocService.Document.Interface.IDocumentService>();
               document = servDoc.Create(document.UserCreateData, document.EntityId, document.DocumentTypeId, document.DocumentLabel);
           }
           catch (System.Data.Entity.Validation.DbEntityValidationException ex1)
           {
               return null;
           }
           catch (Exception ex)
           {
               return null;
           }

           return Ok(document);
        }

        // POST: odata/Documents
        public async Task<IHttpActionResult> Post(Document document)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Add create logic here.

            // return Created(document);
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        // PATCH: odata/Documents(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] System.Guid key, Delta<Document> delta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Get the entity here.

            // delta.Patch(document);

            // TODO: Save the patched entity.

            // return Updated(document);
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        // DELETE: odata/Documents(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] System.Guid key)
        {
            // TODO: Add delete logic here.

            // return StatusCode(HttpStatusCode.NoContent);
            return StatusCode(HttpStatusCode.NotImplemented);
        }
    }
}
