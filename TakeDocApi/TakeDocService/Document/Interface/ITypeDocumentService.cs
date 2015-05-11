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
        /// Add/Update data field with properties for a document type
        /// </summary>
        /// <param name="typeDocumentId"></param>
        /// <param name="dataFieldRef"></param>
        /// <param name="mandatory"></param>
        /// <param name="delete"></param>
        /// <param name="index"></param>
        /// <param name="entityId"></param>
        /// <param name="userId"></param>
        void AddDataField(Guid typeDocumentId, string dataFieldRef, bool mandatory, bool delete, int? index, Guid entityId, Guid userId);
        /// <summary>
        /// Update document type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="userId"></param>
        void Update(TakeDocModel.TypeDocument type, Guid userId);
        /// <summary>
        /// Add new document type
        /// </summary>
        /// <param name="label"></param>
        /// <param name="entityId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        TakeDocModel.TypeDocument Add(string label, Guid entityId, Guid userId);
        // Delete document type
        void Delete(Guid typeDocumentId, Guid userId, Guid entityId);
        /// <summary>
        /// Add backoffice user (administrator) for a document type
        /// </summary>
        /// <param name="param name="userIdToAdd"">user id to add backoffice role</param>
        /// <param name="typeDocumentId"></param>
        /// <param name="entityId"></param>
        /// <param name="userIdUpdater">user id who update data</param>
        void AddBackOfficeUser(Guid userIdToAdd, Guid typeDocumentId, Guid entityId, Guid userIdUpdater);
        /// <summary>
        /// Delete backoffice user (administrator) for a document type
        /// </summary>
        /// <param name="param name="userIdToDel"">user id to remove backoffice role</param>
        /// <param name="typeDocumentId"></param>
        /// <param name="entityId"></param>
        /// <param name="userIdUpdater">user id who update data</param>
        void DeleteBackOfficeUser(Guid userIdToDel, Guid typeDocumentId, Guid entityId, Guid userIdUpdater);
        ICollection<TakeDocModel.UserTk> GetBackOfficeUser(Guid typeDocumentId, Guid entityId);
    }
}
