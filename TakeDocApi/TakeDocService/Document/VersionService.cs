using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using daDoc = TakeDocDataAccess.Document;
using Utility.MyUnityHelper;

namespace TakeDocService.Document
{
    public class VersionService : BaseService, Interface.IVersionService
    {
        TakeDocDataAccess.Document.Interface.IDaoVersionStoreLocator daoVersionLocator = UnityHelper.Resolve<TakeDocDataAccess.Document.Interface.IDaoVersionStoreLocator>();
        TakeDocDataAccess.DaoBase<TakeDocModel.Status_Version> daoStVersion = new TakeDocDataAccess.DaoBase<TakeDocModel.Status_Version>();
        daDoc.Interface.IDaoVersion daoVersion = UnityHelper.Resolve<daDoc.Interface.IDaoVersion>();
        TakeDocDataAccess.Parameter.Interface.IDaoEntity daoEntity = UnityHelper.Resolve<TakeDocDataAccess.Parameter.Interface.IDaoEntity>();

        Interface.IMetaDataService servMetaData = UnityHelper.Resolve<Interface.IMetaDataService>();
        Interface.IPageService servPage = UnityHelper.Resolve<Interface.IPageService>();
        Interface.IImageService servImage = UnityHelper.Resolve<Interface.IImageService>();

        private TakeDocModel.Version Create(Guid userId, Guid entityId, Guid versionId, Guid documentId, decimal versionNumber)
        {
            return daoVersion.Create(userId, entityId, versionId, documentId, versionNumber);
        }

        public TakeDocModel.Version CreateMajor(Guid userId, Guid entityId, Guid versionId, Guid documentId, Guid typeDocumentId)
        {
            ICollection<TakeDocModel.Version> versions = daoVersion.GetBy(x => x.VersionDocumentId == documentId);
            decimal nVersion = 0;
            if (versions.Count() > 0) {
                TakeDocModel.Version last = versions.OrderByDescending(x => x.VersionNumber).First();
                while(Math.Max(nVersion,last.VersionNumber) == last.VersionNumber) {
                    nVersion = nVersion + 1;
                }
            }

            TakeDocModel.Version version = this.Create(userId, entityId, versionId, documentId, nVersion);
            servMetaData.CreateMetaData(userId, entityId, versionId, typeDocumentId);
            return version;
        }

        public TakeDocModel.Version CreateMinor(Guid userId, Guid entityId, Guid versionId, Guid documentId, Guid typeDocumentId)
        {
            ICollection<TakeDocModel.Version> versions = daoVersion.GetBy(x => x.VersionDocumentId == documentId);
            decimal nVersion = 0;
            if (versions.Count() > 0)
                nVersion = versions.OrderByDescending(x => x.VersionNumber).First().VersionNumber + 0.01M;

            TakeDocModel.Version version = this.Create(userId, entityId, versionId, documentId, nVersion);
            servMetaData.CreateMetaData(userId, entityId, versionId, typeDocumentId);
            return version;
        }

        private void SetStatus(TakeDocModel.Version version, string statusRef)
        {
            TakeDocModel.Status_Version stVersion = daoStVersion.GetBy(x => x.StatusVersionReference.Trim() == statusRef && x.EntityId == version.EntityId).First();
            version.VersionStatusId = stVersion.StatusVersionId;
            daoVersion.Update(version);
        }

        public void SetStatus(Guid versionId, Guid entityId, string status)
        {
            TakeDocModel.Version version = daoVersion.GetBy(x => x.VersionId == versionId).First();
            this.SetStatus(version, status);
        }

        public ICollection<TakeDocModel.Version> PdfToGenerate(Guid entityId)
        {
            ICollection<TakeDocModel.Version> versions = daoVersion.GetBy(x => x.Status_Version.StatusVersionReference.Equals(TakeDocModel.Status_Version.MetaSend) 
                && x.EntityId == entityId 
                && x.EtatDeleteData == false
                && x.Document.DocumentCurrentVersionId == x.VersionId && x.Document.EtatDeleteData == false
                && x.Document.Status_Document.StatusDocumentReference.Equals(TakeDocModel.Status_Document.MetaSend)).ToList();
            return versions;
        }

        /// <summary>
        /// Generate Pdf and return version concern
        /// </summary>
        /// <returns></returns>
        public ICollection<TakeDocModel.Version> GeneratePdf()
        {
            ICollection<TakeDocModel.Version> back = new List<TakeDocModel.Version>();

            ICollection<TakeDocModel.Entity> entitys = daoEntity.GetBy(x => x.EtatDeleteData == false).ToList();
            foreach (TakeDocModel.Entity entity in entitys)
            {
                ICollection<TakeDocModel.Version> versions = this.PdfToGenerate(entity.EntityId);
                foreach (TakeDocModel.Version version in versions)
                {
                    back.Add(version);
                    this.GeneratePdf(version.VersionId, version.EntityId);
                }
            }
            return back;
        }

        public void GeneratePdf(Guid versionId, Guid entityId) {
            TakeDocModel.Version version = daoVersion.GetBy(x => x.VersionId == versionId).First();
            this.GeneratePdf(version);
        }

        public void GeneratePdf(TakeDocModel.Version version)
        {
            TakeDocModel.Entity entity = daoEntity.GetBy(x => x.EntityId == version.EntityId).First();

            ICollection<byte[]> data = new List<byte[]>();
            foreach (TakeDocModel.Page page in version.Page.OrderBy(x => x.PageNumber))
            {
                byte[] img = servPage.GetBinary(page.PageId);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(new System.IO.MemoryStream(img));
                float angle = 0;
                bool ok = float.TryParse(page.PageRotation.ToString(), out angle);
                if (ok) img = servImage.Rotate(bitmap, angle);
                data.Add(img);
            }
            byte[] fullDoc = servImage.GetPdf(data);
            System.IO.FileInfo file = this.GenerateUNC(entity.EntityReference, version.VersionReference, "pdf");
            System.IO.File.WriteAllBytes(file.FullName, fullDoc);

            ICollection<TakeDocModel.View_VersionStoreLocator> locators = new List<TakeDocModel.View_VersionStoreLocator>();
            locators = daoVersionLocator.GetBy(x => x.StreamLocator.ToUpper() == file.FullName.ToUpper());
            version.VersionStreamId = locators.First().StreamId;
            this.SetStatus(version, TakeDocModel.Status_Version.Complete);
        }

        private System.IO.FileInfo GenerateUNC(string entite, string fileName, string extension)
        {
            string storeLocalPath = string.Concat(@"\", entite, @"\", extension);
            string[] arr = storeLocalPath.Split('/');
            string deep = string.Empty;
            foreach (string s in arr)
            {
                if (string.IsNullOrEmpty(s) == false)
                {
                    deep = string.Concat(deep, @"\", s);
                    if (System.IO.Directory.Exists(deep) == false) System.IO.Directory.CreateDirectory(string.Concat(TakeDocModel.Environnement.VersionStoreUNC, @"\", deep));
                }
            }
            return new System.IO.FileInfo(string.Concat(TakeDocModel.Environnement.VersionStoreUNC, @"\", storeLocalPath, @"\", fileName, ".", extension));
        }


        public TakeDocModel.Version GetById(Guid versionId, params System.Linq.Expressions.Expression<Func<TakeDocModel.Version, object>>[] properties)
        {
            ICollection<TakeDocModel.Version> versions = daoVersion.GetBy(x => x.VersionId == versionId, properties);
            if (versions.Count() == 0) return null;
            return versions.First();
        }

        public ICollection<TakeDocModel.Version> GetBy(Expression<Func<TakeDocModel.Version, bool>> where, params Expression<Func<TakeDocModel.Version, object>>[] properties)
        {
            return daoVersion.GetBy(where, properties);
        }

        /// <summary>
        /// Return file in byte array
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public byte[] GetBinaryFile(Guid versionId, Guid entityId)
        {
            ICollection<TakeDocModel.Version> versions = daoVersion.GetBy(x => x.VersionId == versionId && x.EntityId == entityId);
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
            System.IO.FileInfo file = new System.IO.FileInfo(string.Concat(TakeDocModel.Environnement.PdfTemp,versionId,".pdf"));
            if (System.IO.File.Exists(file.FullName)) System.IO.File.Delete(file.FullName);
            System.IO.File.WriteAllBytes(file.FullName, data);
            return file.Name;
        }
    }
}
