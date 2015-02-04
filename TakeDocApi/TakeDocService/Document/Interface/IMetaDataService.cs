using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocService.Document.Interface
{
    public interface IMetaDataService
    {
        void CreateMetaData(Guid userId, Guid entityId, Guid documentId, Guid typeDocumentId);
        bool IsValid(string typeName, string value, bool required);
        void SetMetaData(Guid userId, Guid entityId, Guid documentId, IDictionary<string, string> metadatas);
    }
}
