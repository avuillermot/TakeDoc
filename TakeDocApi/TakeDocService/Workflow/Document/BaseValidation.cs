using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakeDocDataAccess.Document.Interface;
using TakeDocDataAccess.Workflow.Interface;
using Utility.MyUnityHelper;
using System.Transactions;
using TakeDocService.Document.Interface;

namespace TakeDocService.Workflow.Document
{
    public abstract class BaseValidation
    {
        protected TakeDocModel.TakeDocEntities1 context = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocModel.TakeDocEntities1>();

        protected IDaoDocument daoDocument = UnityHelper.Resolve<IDaoDocument>();
        protected IDaoWorkflow daoWorkflow = UnityHelper.Resolve<IDaoWorkflow>();

        protected Print.Interface.IReportVersionService servReportVersion = UnityHelper.Resolve<Print.Interface.IReportVersionService>();
        protected IMetaDataService servMeta = UnityHelper.Resolve<IMetaDataService>();
        protected TakeDocService.Workflow.Document.Interface.IStatus servStatus = new TakeDocService.Workflow.Document.Status();

        protected void SetStatus(TakeDocModel.Document document, string status, Guid userId)
        {
            // we update only if status are different
            bool ok = (document.Status_Document.StatusDocumentReference.Equals(status) == false);
            // we update only if the new status is allow
            if (ok == true) ok = servStatus.CheckNewStatus(document.Status_Document.StatusDocumentReference, status);
            if (ok == true) servStatus.SetStatus(document, status, userId, true);
        }
    }
}
