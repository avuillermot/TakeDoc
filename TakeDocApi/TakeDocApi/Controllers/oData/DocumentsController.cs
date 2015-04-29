﻿using System;
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

        // PUT: odata/Documents(5)
        [EnableQuery]
        public IHttpActionResult Put([FromODataUri] string key, Document document)
        {
           if (!ModelState.IsValid) return BadRequest(ModelState);

           try
           {          
               TakeDocService.Document.Interface.IDocumentService servDoc = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocService.Document.Interface.IDocumentService>();
               document = servDoc.Create(document.UserCreateData, document.EntityId, document.DocumentTypeId, document.DocumentLabel);
           }
           catch (System.Data.Entity.Validation.DbEntityValidationException ex1)
           {
               return BadRequest(ex1.Message);
           }
           catch (Exception ex)
           {
               return BadRequest(ex.Message);
           }

           return Ok(document);
        }
    }
}
