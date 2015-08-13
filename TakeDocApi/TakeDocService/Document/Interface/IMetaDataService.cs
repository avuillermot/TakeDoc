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
        /// <summary>
        /// Check if value is valid
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="value"></param>
        /// <param name="required"></param>
        /// <returns></returns>
        bool BeProven(string typeName, string value, bool required);
        void SetMetaData(Guid userId, TakeDocModel.Document document, TakeDocModel.Entity entity, Newtonsoft.Json.Linq.JArray jsonMetaData);
        ICollection<TakeDocModel.MetaData> GetByVersion(Guid versionId, Guid entityId);
        ICollection<object> GetJson(ICollection<TakeDocModel.MetaData> metadatas);
        ICollection<TakeDocModel.Dto.Document.ReadOnlyMetadata> GetReadOnlyMetaData(Guid versionId, Guid entityId);
        ICollection<TakeDocModel.Dto.Document.ReadOnlyMetadata> GetReadOnlyMetaData(TakeDocModel.Version version);
        void Delete(Guid versionId, Guid entityId, Guid userId);
        /// <summary>
        /// Check if metadata is valid (type, mandatory, .....)
        /// </summary>
        /// <param name="document"></param>
        /// <param name="metadatas"></param>
        /// <returns></returns>
        bool BeProven(TakeDocModel.Document document, ICollection<TakeDocModel.MetaData> metadatas);
    }
}
