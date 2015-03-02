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

namespace TakeDocApi.Controllers.oData
{
    public class DocumentsController : oDataBase<Document>
    {
        private static ODataValidationSettings _validationSettings = new ODataValidationSettings();
        
        // GET: odata/Documents
        [EnableQuery]
        public IHttpActionResult GetDocuments(ODataQueryOptions<Document> queryOptions)
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
    }
}
