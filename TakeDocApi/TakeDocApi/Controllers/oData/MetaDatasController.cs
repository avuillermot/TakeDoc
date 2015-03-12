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
    /*
    La classe WebApiConfig peut exiger d'autres modifications pour ajouter un itinéraire à ce contrôleur. Fusionnez ces instructions dans la méthode Register de la classe WebApiConfig, le cas échéant. Les URL OData sont sensibles à la casse.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using TakeDocModel;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<MetaData>("MetaDatas");
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class MetaDatasController : ODataController
    {
        private static ODataValidationSettings _validationSettings = new ODataValidationSettings();

        // GET: odata/MetaDatas
        public IHttpActionResult GetMetaDatas(ODataQueryOptions<MetaData> queryOptions)
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

            // return Ok<IEnumerable<MetaData>>(metaDatas);
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        // GET: odata/MetaDatas(5)
        public IHttpActionResult GetMetaData([FromODataUri] System.Guid key, ODataQueryOptions<MetaData> queryOptions)
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

            // return Ok<MetaData>(metaData);
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        // PUT: odata/MetaDatas(5)
        public IHttpActionResult Put([FromODataUri] System.Guid key, Delta<MetaData> delta)
        {
            Validate(delta.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Get the entity here.

            // delta.Put(metaData);

            // TODO: Save the patched entity.

            // return Updated(metaData);
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        // POST: odata/MetaDatas
        public IHttpActionResult Post(MetaData metaData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Add create logic here.

            // return Created(metaData);
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        // PATCH: odata/MetaDatas(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] System.Guid key, Delta<MetaData> delta)
        {
            Validate(delta.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Get the entity here.

            // delta.Patch(metaData);

            // TODO: Save the patched entity.

            // return Updated(metaData);
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        // DELETE: odata/MetaDatas(5)
        public IHttpActionResult Delete([FromODataUri] System.Guid key)
        {
            // TODO: Add delete logic here.

            // return StatusCode(HttpStatusCode.NoContent);
            return StatusCode(HttpStatusCode.NotImplemented);
        }
    }
}
