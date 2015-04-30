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
    }
}
