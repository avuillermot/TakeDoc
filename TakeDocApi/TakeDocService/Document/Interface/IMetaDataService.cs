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
        bool IsValid(string typeName, string value, bool required);
        void SetMetaData(Guid userId, TakeDocModel.Document document, TakeDocModel.Entity entity, string json);
        ICollection<TakeDocModel.MetaData> GetByVersion(Guid versionId, Guid entityId);
        ICollection<TakeDocModel.Dto.Document.ReadOnlyMetadata> GetReadOnlyMetaData(Guid versionId, Guid entityId);
        ICollection<TakeDocModel.Dto.Document.ReadOnlyMetadata> GetReadOnlyMetaData(TakeDocModel.Version version);
        void Delete(Guid versionId, Guid entityId, Guid userId);
    }
}
