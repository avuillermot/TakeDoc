using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TakeDocService.Document.Interface;

namespace TakeDocApi.Controllers
{
    [RoutePrefix("Version")]
    public class VersionController : ApiController
    {
        [HttpGet]
        [Route("GetMetaData/{documentId}")]
        public ICollection<object> GetMetaData(Guid documentId)
        {
            IDocumentService servDocument = Utility.MyUnityHelper.UnityHelper.Resolve<IDocumentService>();
            IVersionService servVersion = Utility.MyUnityHelper.UnityHelper.Resolve<IVersionService>();

            TakeDocModel.Document document = servDocument.GetById(documentId, x => x.Version);
            if (document.LastVersion == null) return null;

            TakeDocModel.Version version = servVersion.GetById(document.LastVersion.VersionId, x => x.MetaData);
            var req = from m in version.MetaData.Where(x => x.EtatDeleteData == false) select new 
                { MetaDataName = m.MetaDataName, MetaDataValue = m.MetaDataValue, DataFieldTypeId = m.DataField.DataFieldTypeId,
                MetaDataMandatory = m.DataField.DataFieldMandatory, MetaDataInputType = m.DataField.DataFieldType.DataFieldInputType};
            return req.ToList<object>();
        }
    }
}
