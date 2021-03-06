﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocDataAccess.Workflow.Interface
{
    public interface IDaoWorkflow
    {
        /// <summary>
        /// Check if all required validation are ok
        /// </summary>
        /// <param name="VersionId"></param>
        /// <returns></returns>
        bool IsAllApprove(Guid VersionId, Guid entityId);
        void Add(TakeDocModel.Workflow workflow);
        ICollection<TakeDocModel.Workflow> GetBy(Expression<Func<TakeDocModel.Workflow, bool>> where, params Expression<Func<TakeDocModel.Workflow, object>>[] properties);
        void SetAnswer(TakeDocModel.Workflow workflow, Guid answer, Guid userId, string comment);
        /// <summary>
        /// Cancel all next step if a no go on answer
        /// </summary>
        /// <param name="versionId"></param>
        void CancelWorkflowStep(Guid versionId);
    }
}
