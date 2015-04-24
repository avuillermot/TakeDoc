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
        TakeDocDataAccess.DaoBase<TakeDocModel.Status_Version> daoStVersion = new TakeDocDataAccess.DaoBase<TakeDocModel.Status_Version>();
        daDoc.Interface.IDaoVersion daoVersion = UnityHelper.Resolve<daDoc.Interface.IDaoVersion>();

        Interface.IMetaDataService servMetaData = UnityHelper.Resolve<Interface.IMetaDataService>();
        Interface.IPageService servPage = UnityHelper.Resolve<Interface.IPageService>();

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

        public void SetStatus(TakeDocModel.Version version, string statusRef, Guid userId)
        {
            TakeDocModel.Status_Version stVersion = daoStVersion.GetBy(x => x.StatusVersionReference.Trim() == statusRef && x.EntityId == version.EntityId).First();
            version.VersionStatusId = stVersion.StatusVersionId;
            version.UserUpdateData = userId;
            version.DateUpdateData = System.DateTimeOffset.UtcNow;

            daoVersion.Update(version);
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

        public void Delete(Guid documentId, Guid entityId, Guid userId)
        {
            ICollection<TakeDocModel.Version> versions = daoVersion.GetBy(x => x.Document.DocumentId == documentId);
            foreach (TakeDocModel.Version version in versions) {
                servMetaData.Delete(version.VersionId, entityId, userId);
                version.EtatDeleteData = true;
                version.DateDeleteData = System.DateTime.UtcNow;
                version.UserDeleteData = userId;
                daoVersion.Update(version);
            }
        }

        public ICollection<object> GetPages(Guid versionId, Guid entityId, Guid userId)
        {
            ICollection<object> back = new List<object>();
            TakeDocModel.Version version = daoVersion.GetBy(x => x.VersionId == versionId && x.EntityId == entityId, x => x.Page).First();
            foreach(TakeDocModel.Page page in version.Page.Where(x => x.EtatDeleteData == false)) {
                var current = new {
                    index = page.PageNumber,
                    base64Image = servPage.GetBase64(page.PageId),
                    rotation = page.PageRotation
                };
                back.Add(current);
            }
            return back.ToArray();
        }

        public void Update(TakeDocModel.Version version)
        {
            daoVersion.Update(version);
        }
    }
}
