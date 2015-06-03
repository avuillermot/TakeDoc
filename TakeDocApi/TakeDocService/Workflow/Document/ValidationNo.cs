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
            TakeDocModel.Version version = document.LastVersion;
            this.SetStatus(document, TakeDocModel.Status_Document.Complete, userId );
            servReportVersion.Generate(version.VersionId, version.EntityId);
            if (this.daoWorkflow.IsAllApprove(version.VersionId, version.EntityId)) this.SetStatus(document, TakeDocModel.Status_Document.Archive, userId);
            return true;
        }

        public void Answer(Guid versionId, Guid workflowId, Guid entityId, string answerRef, TakeDocModel.UserTk user)
        {
            throw new NotImplementedException("not required in this worflow type");
        }
    }
}
