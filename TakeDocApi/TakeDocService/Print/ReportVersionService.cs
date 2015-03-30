using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.MyUnityHelper;
using dataDoc = TakeDocDataAccess.Document;

namespace TakeDocService.Print
{
    public class ReportVersionService : BaseService, Interface.IReportVersionService
    {
        Document.Interface.IVersionService servVersion = UnityHelper.Resolve<Document.Interface.IVersionService>();

        dataDoc.Interface.IDaoVersionStoreLocator daoVersionLocator = UnityHelper.Resolve<dataDoc.Interface.IDaoVersionStoreLocator>();
        
        /// <summary>
        /// Return file in byte array
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public byte[] GetBinaryFile(Guid versionId, Guid entityId)
        {
            ICollection<TakeDocModel.Version> versions = servVersion.GetBy(x => x.VersionId == versionId && x.EntityId == entityId);
            if (versions.Count() == 0)
            {
                string msg = string.Format("Unknow version {0} for entity {1}", versionId, entityId);
                base.Logger.Error(msg);
                throw new Exception(msg);
            }
            Guid? streamId = versions.First().VersionStreamId;
            ICollection<TakeDocModel.View_VersionStoreLocator> locators = daoVersionLocator.GetBy(x => x.StreamId == streamId);
            if (locators.Count() == 0)
            {
                string msg = string.Format("Unknow locator for version {0} for entity {1}", versionId, entityId);
                base.Logger.Error(msg);
                throw new Exception(msg);
            }
            return System.IO.File.ReadAllBytes(locators.First().StreamLocator);
        }

        public string GetUrlFile(Guid versionId, Guid entityId)
        {
            byte[] data = this.GetBinaryFile(versionId, entityId);
            System.IO.FileInfo file = new System.IO.FileInfo(string.Concat(TakeDocModel.Environnement.TempDirectory, versionId, ".pdf"));
            if (System.IO.File.Exists(file.FullName)) System.IO.File.Delete(file.FullName);
            System.IO.File.WriteAllBytes(file.FullName, data);
            return file.Name;
        }
    }
}
