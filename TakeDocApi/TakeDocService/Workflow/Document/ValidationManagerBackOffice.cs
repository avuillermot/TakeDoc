using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocService.Workflow.Document
{
    public class ValidationManagerBackOffice : BaseValidation, Interface.IValidation
    {
        public bool Execute(TakeDocModel.Document document, TakeDocModel.UserTk user)
        {
            this.SetStatus(document, TakeDocModel.Status_Document.Complete, user.UserTkId);
            base.SetManagerValidation(document, user, 0);
            base.SetTypeDocumentValidation(document, 1);
            this.SetStatus(document, TakeDocModel.Status_Document.ToValidate, user.UserTkId);
            daoDocument.Update(document);
            servReportVersion.Generate(document.DocumentCurrentVersionId.Value, document.EntityId);
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
