using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TakeDocService.Document.Interface;

namespace TakeDocApi.Controllers.AutoComplete
{
    [RoutePrefix("Client")]
    public class ClientController : ApiController
    {
        [HttpGet]
        [Route("Cegid/{entityId}/{value}")]
        public HttpResponseMessage SetSend(Guid entityId, string value)
        {
            ITypeDocumentService servTypeDocument = Utility.MyUnityHelper.UnityHelper.Resolve<ITypeDocumentService>();
            try
            {
                ICollection<TakeDocModel.TypeDocument> types = servTypeDocument.GetBy(x => (x.TypeDocumentLabel.Contains(value) || x.TypeDocumentReference.Contains(value)) && x.EntityId == entityId);
                var req = from type in types
                          select new
                          {
                              key = type.TypeDocumentId,
                              text = type.TypeDocumentLabel
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
