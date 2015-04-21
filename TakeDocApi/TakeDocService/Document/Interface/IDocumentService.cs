using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace TakeDocService.Document.Interface
{
    public interface IDocumentService
    {
        TakeDocModel.Document Create(Guid userId, Guid entityId, Guid typeDocumentId, string documentLabel);
        void AddPage(Guid userId, Guid entityId, Guid documentId, string imageString, string extension, int rotation);
        void AddVersionMajor(Guid userId, Guid entityId, Guid documentId);
        void AddVersionMinor(Guid userId, Guid entityId, Guid documentId);
        void SetStatus(Guid document, string status, Guid userId, bool updateStatusVersion);
        TakeDocModel.Document GetById(Guid documentId, params Expression<Func<TakeDocModel.Document, object>>[] properties);
        void SetMetaData(Guid userId, Guid entityId, Guid versionId, IDictionary<string, string> metadatas);
        /// <summary>
        /// Delete document, all his version and all metadata
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="entityId"></param>
        /// <param name="userId"></param>
        void Delete(Guid documentId, Guid entityId, Guid userId);
    }
}
