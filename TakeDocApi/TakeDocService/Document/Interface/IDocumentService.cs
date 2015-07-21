using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace TakeDocService.Document.Interface
{
    public interface IDocumentService
    {
        /// <summary>
        /// Create a document type
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="entityId"></param>
        /// <param name="typeDocumentId"></param>
        /// <param name="documentLabel"></param>
        /// <returns></returns>
        TakeDocModel.Document Create(Guid userId, Guid entityId, Guid typeDocumentId, string documentLabel);
        /// <summary>
        /// Add page to an document
        /// </summary>
        /// <param name="userId">user who create page</param>
        /// <param name="entityId">entity of the document/page</param>
        /// <param name="documentId"></param>
        /// <param name="imageString">image in base64 string format</param>
        /// <param name="extension">extension og image (ie : png)</param>
        /// <param name="rotation">rotation in degree to have image in good direction</param>
        void AddPage(Guid userId, Guid entityId, Guid documentId, string imageString, string extension, int rotation);
        void AddVersionMajor(Guid userId, Guid entityId, Guid documentId);
        void AddVersionMinor(Guid userId, Guid entityId, Guid documentId);
        TakeDocModel.Document GetById(Guid documentId, params Expression<Func<TakeDocModel.Document, object>>[] properties);
        void SetMetaData(Guid userId, Guid entityId, Guid versionId, string json);
        void SetTitle(string title, Guid versionId, Guid userId, Guid entityId);
        /// <summary>
        /// Delete document, all his version and all metadata
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="entityId"></param>
        /// <param name="userId"></param>
        void Delete(Guid documentId, Guid entityId, Guid userId);
        ICollection<TakeDocModel.View_DocumentExtended> Search(string title, Guid typeDocumentId, ICollection<TakeDocModel.Dto.Document.SearchMetadata> metadatas, Guid userId, Guid entityId);
    }
}
