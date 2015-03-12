using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TakeDocService.Document.Interface;

namespace TakeDocApi.Controllers
{
    [RoutePrefix("MetaData")]
    public class MetaDataController : ApiController
    {
        [HttpGet]
        [Route("Version/{versionId}/{entityId}")]
        public ICollection<object> GetMetaData(Guid versionId, Guid entityId)
        {
            IMetaDataService servMetaData = Utility.MyUnityHelper.UnityHelper.Resolve<IMetaDataService>();
            
            ICollection<TakeDocModel.MetaData> metadatas = servMetaData.GetByVersion(versionId, entityId);
            var req = from metadata in metadatas select new {
                metadata.MetaDataId, metadata.MetaDataDisplayIndex, metadata.MetaDataName, 
                metadata.MetaDataValue, metadata.DataField.DataFieldMandatory, 
                metadata.DataField.DataFieldType.DataFieldInputType,
                metadata.DataField.DataFieldLabel
            };
            return req.ToList<object>();
        }
    }
}
