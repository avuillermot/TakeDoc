using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocService.Document
{
    public class MetaDataFileService : BaseService, Interface.IMetaDataFileService
    {
        private TakeDocDataAccess.Document.Interface.IDaoMetaDataFile daoMdFile = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocDataAccess.Document.Interface.IDaoMetaDataFile>();

        public System.IO.FileInfo GetFile(string fullName)
        {
            System.IO.FileInfo f;
            try
            {
                f = new System.IO.FileInfo(fullName);
            }
            catch (Exception ex)
            {
                f = new System.IO.FileInfo(new Uri(fullName).LocalPath);
            }
            return f;
        }

        public TakeDocModel.MetaDataFile Create(string fullName, byte[] data, Guid metadataId, Guid userId, TakeDocModel.Entity entity)
        {
            ICollection<TakeDocModel.MetaDataFile> mFiles = daoMdFile.GetBy(x => x.MetaDataId == metadataId && x.EntityId == entity.EntityId);
            System.IO.FileInfo f = this.GetFile(fullName);
            foreach (TakeDocModel.MetaDataFile mFile in mFiles)
            {
                System.IO.FileInfo delete = this.GenerateUNC(entity.EntityReference, TakeDocModel.Environnement.MetaDataFileStoreUNC, mFile.MetaDataFileReference, new System.IO.FileInfo(mFile.MetaDataFileName).Extension.Replace(".", string.Empty), false);
                if (delete.Exists) delete.Delete();
                daoMdFile.Delete(mFile);
            }
                       
            TakeDocModel.MetaDataFile back = new TakeDocModel.MetaDataFile()
            {
                MetaDataFilePath = fullName,
                MetaDataFileName = f.Name,
                MetaDataId = metadataId,
                UserCreateData = userId,
                DateCreateData = System.DateTimeOffset.UtcNow,
                EtatDeleteData = false,
                EntityId = entity.EntityId
            };
            try
            {
                daoMdFile.Add(back);
                System.IO.FileInfo file = this.GenerateUNC(entity.EntityReference, TakeDocModel.Environnement.MetaDataFileStoreUNC, back.MetaDataFileReference, f.Extension.Replace(".", string.Empty),true);
                System.IO.File.WriteAllBytes(file.FullName, data);
                
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex1)
            {
                base.Logger.Error(ex1);
                throw ex1;
            }
            catch (Exception ex)
            {
                base.Logger.Error(ex);
                throw ex;
            }
            return back;
        }

        private System.IO.FileInfo GenerateUNC(string entite, string store, string fileName, string extension, bool create)
        {
            string storeLocalPath = string.Concat(@"\", entite, @"\", extension);
            base.Logger.DebugFormat("GenerateUNC path in [{0}]", storeLocalPath);
            string[] arr = storeLocalPath.Split('\\');
            string deep = string.Empty;
            foreach (string s in arr)
            {
                if (string.IsNullOrEmpty(s) == false)
                {
                    deep = string.Concat(deep, @"\", s);
                    if (System.IO.Directory.Exists(deep) == false && create) System.IO.Directory.CreateDirectory(string.Concat(TakeDocModel.Environnement.MetaDataFileStoreUNC, @"\", deep));
                }
            }
            return new System.IO.FileInfo(string.Concat(store, @"\", storeLocalPath, @"\", fileName, ".", extension));
        }
    }
}
