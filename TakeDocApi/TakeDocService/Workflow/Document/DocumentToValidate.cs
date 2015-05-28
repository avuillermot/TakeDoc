using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.MyUnityHelper;

namespace TakeDocService.Workflow.Document
{
    public class DocumentToValidate : Interface.IDocumentToValidate
    {
        TakeDocModel.TakeDocEntities1 context = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocModel.TakeDocEntities1>();

        public ICollection<TakeDocModel.DocumentToValidate> GetByManager(Guid userId)
        {
            return context.GetDocumentToValidate(userId).Where(x => x.TypeValidation == "MANAGER").ToList();
        }

        public ICollection<TakeDocModel.DocumentToValidate> GetByTypeDocument(Guid userId)
        {
            return context.GetDocumentToValidate(userId).Where(x => x.TypeValidation == "TYPE_DOCUMENT").ToList();
        }

        public ICollection<TakeDocModel.DocumentToValidate> GetAll(Guid userId)
        {
            return context.GetDocumentToValidate(userId).ToList();
        }

        public ICollection<object> GetHistorique(Guid documentId)
        {
            var req = from w in context.Workflow
                      join u in context.UserTk on w.WorkflowUserId equals u.UserTkId into wu
                      from u in wu
                      select new { fullName = u.UserTkFirstName + u.UserTkLastName };
            return req.ToList<object>();
        }
    }
}
