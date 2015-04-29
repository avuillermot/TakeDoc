using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocService.Workflow.Document
{
    public class ValidationManager : BaseValidation, Interface.IValidation
    {
        public bool Execute(TakeDocModel.Document document, TakeDocModel.UserTk user, IDictionary<string, string> metadatas)
        {
            return this.Execute(document, user.UserTkId, metadatas);
        }

        public bool Execute(TakeDocModel.Document document, Guid userId, IDictionary<string, string> metadatas)
        {
            this.SetStatus(document, TakeDocModel.Status_Document.ToValidate, userId, metadatas);
            servReportVersion.Generate(document.DocumentCurrentVersionId.Value, document.EntityId);
            return true;
        }
    }
}
