using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocService.Document.Interface
{
    public interface IMetaDataService
    {
        void CreateMetaData(Guid userId, Guid entityId, Guid versionId, Guid typeDocumentId);
        void Valid(string typeName, string value, bool required);
        bool IsValid(string typeName, string value, bool required);
        /// <summary>
        /// Update metadata
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="entityId"></param>
        /// <param name="versionId"></param>
        /// <param name="metadatas">where key is metadata name and value is the new value</param>
        void SetMetaData(Guid userId, Guid entityId, Guid versionId, IDictionary<string, string> metadatas);
        ICollection<TakeDocModel.MetaData> GetByVersion(Guid versionId, Guid entityId);
    }
}
