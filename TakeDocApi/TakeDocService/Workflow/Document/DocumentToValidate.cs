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
    }
}
