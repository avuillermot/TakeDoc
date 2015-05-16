using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocService.Workflow.Document.Interface
{
    public interface IStatus
    {
        void SetStatus(Guid documentId, string status, Guid userId, bool updateStatusVersion);
        void SetStatus(TakeDocModel.Document document, string status, Guid userId, bool updateStatusVersion);
        /// <summary>
        /// Check if new status is allow
        /// </summary>
        /// <param name="oldStatus"></param>
        /// <param name="newStatus"></param>
        /// <returns></returns>
        bool CheckNewStatus(string oldStatus, string newStatus);
        /// <summary>
        /// Return status history for a document
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        ICollection<TakeDocModel.DocumentStatusHisto> GetStatus(Guid documentId, Guid entityId);
        /// <summary>
        /// Return status history for a document
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        ICollection<TakeDocModel.DocumentStatusHisto> GetStatus(TakeDocModel.Document document);
        /// <summary>
        /// Return all status allow for an entity
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        ICollection<TakeDocModel.Status_Document> GetStatus(Guid entityId);
    }
}
