﻿using System;
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
            TakeDocModel.Version version = document.LastVersion;
            this.SetStatus(document, TakeDocModel.Status_Document.Complete, user.UserTkId);
            this.SetStatus(document, TakeDocModel.Status_Document.ToValidate, user.UserTkId);
            servReportVersion.Generate(version.VersionId, version.EntityId);
            if (this.daoWorkflow.IsAllApprove(version.VersionId, version.EntityId)) this.SetStatus(document, TakeDocModel.Status_Document.Archive, user.UserTkId);
            return true;
        }
    }
}
