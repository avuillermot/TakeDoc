using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocService.Workflow.Document
{
    public class ValidationBackOffice : BaseValidation, Interface.IValidation
    {
        public bool Execute(TakeDocModel.Document document, TakeDocModel.UserTk user)
        {
            this.SetStatus(document, TakeDocModel.Status_Document.Complete, user.UserTkId);
            this.SetStatus(document, TakeDocModel.Status_Document.ToValidate, user.UserTkId);
            base.SetTypeDocumentValidation(document, "BACKOFFICE", 0);
            daoDocument.Update(document);
            servReportVersion.Generate(document.DocumentCurrentVersionId.Value, document.EntityId);
            return true;
        }
    }
}
