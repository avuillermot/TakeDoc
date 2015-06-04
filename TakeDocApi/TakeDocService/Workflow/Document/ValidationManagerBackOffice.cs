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
            this.SetStatus(document, TakeDocModel.Status_Document.ToValidate, user.UserTkId);
            base.SetManagerValidation(document, user, "MANAGER-BACKOFFICE", 0);
            base.SetTypeDocumentValidation(document, "MANAGER-BACKOFFICE", 1);
            daoDocument.Update(document);
            servReportVersion.Generate(document.DocumentCurrentVersionId.Value, document.EntityId);
            return true;
        }

        public void Answer(Guid versionId, Guid workflowId, Guid entityId, string answerRef, TakeDocModel.UserTk user)
        {
            /*ICollection<TakeDocModel.Workflow> wfs = this.daoWorkflow.GetBy(x => x.WorkflowId == workflowId && x.EntityId == entityId && x.WorkflowAnswerId == null);
            TakeDocModel.WorkflowAnswer answer = daoWfAnswer.GetBy(x => x.WorkflowAnswerReference == answerRef).First();

            this.daoWorkflow.SetAnswer(wfs, answer, user.UserTkId);
            if (this.daoWorkflow.IsAllApprove(versionId, entityId))
            {
                TakeDocModel.Document document = daoDocument.GetBy(x => x.DocumentCurrentVersionId == versionId && x.EntityId == entityId).First();
                this.SetStatus(document, TakeDocModel.Status_Document.Archive, user.UserTkId);
            }*/
        }

        public void Refuse(TakeDocModel.Document document, TakeDocModel.UserTk user)
        {
            throw new NotImplementedException();
        }
    }
}
