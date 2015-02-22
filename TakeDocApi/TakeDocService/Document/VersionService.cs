using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using daDoc = TakeDocDataAccess.Document;
using Utility.MyUnityHelper;

namespace TakeDocService.Document
{
    public class VersionService : BaseService, Interface.IVersionService
    {
        TakeDocDataAccess.DaoBase<TakeDocModel.View_VersionStoreLocator> dao = new TakeDocDataAccess.DaoBase<TakeDocModel.View_VersionStoreLocator>();
        TakeDocDataAccess.DaoBase<TakeDocModel.Statut_Version> daoStVersion = new TakeDocDataAccess.DaoBase<TakeDocModel.Statut_Version>();
        daDoc.Interface.IDaoVersion daoVersion = UnityHelper.Resolve<daDoc.Interface.IDaoVersion>();

        Interface.IPageService servPage = UnityHelper.Resolve<Interface.IPageService>();
        Interface.IImageService servImage = UnityHelper.Resolve<Interface.IImageService>();

        private TakeDocModel.Version Create(Guid userId, Guid entityId, Guid versionId, Guid documentId, decimal versionNumber)
        {
            return daoVersion.Create(userId, entityId, versionId, documentId, versionNumber);
        }

        public TakeDocModel.Version CreateMajor(Guid userId, Guid entityId, Guid versionId, Guid documentId)
        {
            ICollection<TakeDocModel.Version> versions = daoVersion.GetBy(x => x.VersionDocumentId == documentId);
            decimal nVersion = 0;
            if (versions.Count() > 0) {
                TakeDocModel.Version last = versions.OrderByDescending(x => x.VersionNumber).First();
                while(Math.Max(nVersion,last.VersionNumber) == last.VersionNumber) {
                    nVersion = nVersion + 1;
                }
            }

            return this.Create(userId, entityId, versionId, documentId, nVersion);
        }

        public TakeDocModel.Version CreateMinor(Guid userId, Guid entityId, Guid versionId, Guid documentId)
        {
            ICollection<TakeDocModel.Version> versions = daoVersion.GetBy(x => x.VersionDocumentId == documentId);
            decimal nVersion = 0;
            if (versions.Count() > 0)
                nVersion = versions.OrderByDescending(x => x.VersionNumber).First().VersionNumber + 0.01M;

            return this.Create(userId, entityId, versionId, documentId, nVersion);
        }

        public void SetReceive(Guid versionId)
        {
            TakeDocModel.Version version = daoVersion.GetBy(x => x.VersionId == versionId).First();
            TakeDocModel.Statut_Version stVersion = daoStVersion.GetBy(x => x.StatutVersionReference.Trim() == "RECEIVE").First();
            this.SetStatut(version, stVersion);
        }

        public void SetReceive(TakeDocModel.Version version)
        {
            TakeDocModel.Statut_Version stVersion = daoStVersion.GetBy(x => x.StatutVersionReference.Trim() == "RECEIVE").First();
            this.SetStatut(version, stVersion);
        }

        private void SetStatut(TakeDocModel.Version version, TakeDocModel.Statut_Version statut)
        {
            ICollection<byte[]> data = new List<byte[]>();
            foreach (TakeDocModel.Page page in version.Page)
            {
                byte[] img = servPage.GetBinary(page.PageId);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(new System.IO.MemoryStream(img));
                img = servImage.Rotate(bitmap);
                data.Add(img);
            }
            byte[] fullDoc = servImage.GetPdfFromJpeg(data);
            System.IO.FileInfo file = this.GenerateUNC("MASTER", version.VersionReference, "pdf");
            System.IO.File.WriteAllBytes(file.FullName, fullDoc);

            ICollection<TakeDocModel.View_VersionStoreLocator> locators = new List<TakeDocModel.View_VersionStoreLocator>();
            locators = dao.GetBy(x => x.StreamLocator.ToUpper() == file.FullName.ToUpper());
            version.VersionStreamId = locators.First().StreamId;

            if (version.Page.Count() == 0) version = daoVersion.GetBy(x => x.VersionId == version.VersionId, x => x.Page).First();
            version.VersionStatutId = statut.StatutVersionId;
            daoVersion.Update(version);
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
    }
}
