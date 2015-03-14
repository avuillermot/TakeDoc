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
                id =  metadata.MetaDataId,
                index = metadata.MetaDataDisplayIndex, 
                name = metadata.MetaDataName, 
                value = metadata.MetaDataValue, 
                mandatory = metadata.MetaDataMandatory,
                type = metadata.DataField.DataFieldType.DataFieldInputType,
                label = metadata.DataField.DataFieldLabel
            };
            return req.ToList<object>();
        }

        [HttpPut]
        [Route("{versionId}/{userId}/{entityId}")]
        public void SetMetaData(Guid versionId, Guid userId, Guid entityId, [FromBody]string value)
        {
            IMetaDataService servMetaData = Utility.MyUnityHelper.UnityHelper.Resolve<IMetaDataService>();
            Newtonsoft.Json.Linq.JArray data = Newtonsoft.Json.Linq.JArray.Parse(value);
            IDictionary<string,string> metadatas = new Dictionary<string,string>();
            foreach (Newtonsoft.Json.Linq.JObject obj in data)
            {
                string name = obj.Value<string>("name");
                string input = obj.Value<string>("type").ToUpper();

                if (input.Equals("DATE"))
                {
                    DateTimeOffset date = DateTime.SpecifyKind(obj.Value<DateTime>("value"), DateTimeKind.Utc);

                    metadatas.Add(name, date.ToString());
                }
                else metadatas.Add(name, obj.Value<string>("value"));
            }
            servMetaData.SetMetaData(userId, entityId, versionId, metadatas);
        }
    }
}
