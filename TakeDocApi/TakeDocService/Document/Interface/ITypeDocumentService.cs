using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocService.Document.Interface
{
    public interface ITypeDocumentService
    {
        ICollection<TakeDocModel.TypeDocument> Get(Guid userId, Guid entityId);
        ICollection<TakeDocModel.TypeDocument> GetBy(Expression<Func<TakeDocModel.TypeDocument, bool>> where, params Expression<Func<TakeDocModel.TypeDocument, object>>[] properties);
        /// <summary>
        /// Add datafield to a document type with properties
        /// </summary>
        /// <param name="typeDocumentRef"></param>
        /// <param name="dataFieldRef"></param>
        /// <param name="mandatory"></param>
        /// <param name="delete"></param>
        /// <param name="index"></param>
        /// <param name="entityRef"></param>
        /// <param name="userRef"></param>
        void AddDataField(string typeDocumentRef, string dataFieldRef, bool mandatory, bool delete, int? index, string entityRef, string userRef);
        /// <summary>
        /// Add data field to a document type with properties
        /// </summary>
        /// <param name="typeDocumentId"></param>
        /// <param name="dataFieldRef"></param>
        /// <param name="mandatory"></param>
        /// <param name="delete"></param>
        /// <param name="index"></param>
        /// <param name="entityId"></param>
        /// <param name="userId"></param>
        void AddDataField(Guid typeDocumentId, string dataFieldRef, bool mandatory, bool delete, int? index, Guid entityId, Guid userId);
        void Update(TakeDocModel.TypeDocument type, Guid userId);
        TakeDocModel.TypeDocument Add(string label, Guid entityId, Guid userId);
        void Delete(Guid typeDocumentId, Guid userId, Guid entityId);
        /// <summary>
        /// Add backoffice user (administrator) for a document type
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="typeDocumentId"></param>
        /// <param name="entityId"></param>
        void AddBackofficeUser(Guid userId, Guid typeDocumentId, Guid entityId);
        /// <summary>
        /// Delete backoffice user (administrator) for a document type
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="typeDocumentId"></param>
        /// <param name="entityId"></param>
        void DeleteBackofficeUser(Guid userId, Guid typeDocumentId, Guid entityId);
    }
}
