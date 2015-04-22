using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.MyUnityHelper;
using dataDoc = TakeDocDataAccess.Document;
using serviceDoc = TakeDocService.Document;

namespace TakeDocService.Workflow.Document
{
    public class SetStatusToValidate : Interface.ISetStatusToValidate
    {
        TakeDocDataAccess.Parameter.Interface.IDaoEntity daoEntity = UnityHelper.Resolve<TakeDocDataAccess.Parameter.Interface.IDaoEntity>();
        dataDoc.Interface.IDaoVersionStoreLocator daoVersionLocator = UnityHelper.Resolve<dataDoc.Interface.IDaoVersionStoreLocator>();

        serviceDoc.Interface.IDocumentService servDocument = UnityHelper.Resolve<serviceDoc.Interface.IDocumentService>();
        serviceDoc.Interface.IVersionService servVersion = UnityHelper.Resolve<serviceDoc.Interface.IVersionService>();
        Print.Interface.IReportVersionService servReportVersion = UnityHelper.Resolve<Print.Interface.IReportVersionService>();

        public void Execute(Guid userId)
        {
            ICollection<TakeDocModel.Entity> entitys = daoEntity.GetBy(x => x.EtatDeleteData == false).ToList();
            foreach (TakeDocModel.Entity entity in entitys)
            {
                ICollection<TakeDocModel.Version> versions = this.Get(entity);
                foreach (TakeDocModel.Version version in versions)
                {
                    bool ok = this.GeneratePdf(version, userId);
                    if (ok) servDocument.SetStatus(version.VersionDocumentId, TakeDocModel.Status_Document.ToValidate, userId, true);
                }
            }
        }

        private ICollection<TakeDocModel.Version> Get(TakeDocModel.Entity entity)
        {
            ICollection<TakeDocModel.Version> versions = servVersion.GetBy(x => x.Status_Version.StatusVersionReference.Equals(TakeDocModel.Status_Version.Complete) 
                && x.EntityId == entity.EntityId 
                && x.EtatDeleteData == false
                && x.Document.DocumentCurrentVersionId == x.VersionId && x.Document.EtatDeleteData == false
                && x.Document.Status_Document.StatusDocumentReference.Equals(TakeDocModel.Status_Document.Complete)).ToList();
            return versions;
        }

       private bool GeneratePdf(TakeDocModel.Version version, Guid userId)
        {
            TakeDocModel.Entity entity = daoEntity.GetBy(x => x.EntityId == version.EntityId).First();

            byte[] data = servReportVersion.Generate(version, entity);
            if (data == null) return false;
            return true;
        }
    }
}
