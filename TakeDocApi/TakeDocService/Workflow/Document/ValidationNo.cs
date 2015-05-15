using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocService.Workflow.Document
{
    public class ValidationNo : BaseValidation, Interface.IValidation
    {
        public bool Execute(TakeDocModel.Document document, TakeDocModel.UserTk user)
        {
            return this.Execute(document, user.UserTkId);
        }

        private bool Execute(TakeDocModel.Document document, Guid userId)
        {
            this.SetStatus(document, TakeDocModel.Status_Document.Complete, userId );
            servReportVersion.Generate(document.DocumentCurrentVersionId.Value, document.EntityId);
            this.SetStatus(document, TakeDocModel.Status_Document.Archive, userId);
            return true;
        }
        
        public void Approve(TakeDocModel.Document document, TakeDocModel.UserTk user)
        {
            throw new NotImplementedException();
        }

        public void Refuse(TakeDocModel.Document document, TakeDocModel.UserTk user)
        {
            throw new NotImplementedException();
        }
    }
}
