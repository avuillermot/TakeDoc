using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocService.Workflow.Document
{
    public class ValidationAuto : BaseValidation, Interface.IValidation
    {
        public bool Execute(TakeDocModel.Document document, TakeDocModel.UserTk user)
        {
            this.SetStatus(document, TakeDocModel.Status_Document.Complete, user.UserTkId);
            this.Approve(document, user);
            servReportVersion.Generate(document.DocumentCurrentVersionId.Value, document.EntityId);
            this.SetStatus(document, TakeDocModel.Status_Document.Archive, user.UserTkId);
            return true;
        }

        public void Approve(TakeDocModel.Document document, TakeDocModel.UserTk user)
        {
            this.SetStatus(document, TakeDocModel.Status_Document.Approve, user.UserTkId);
        }

        public void Refuse(TakeDocModel.Document document, TakeDocModel.UserTk user)
        {
            throw new NotImplementedException();
        }
    }
}
