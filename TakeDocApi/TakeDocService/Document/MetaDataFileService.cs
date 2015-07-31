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

        public void Delete(Guid metaDataId, Guid userId)
        {
            daoMdFile.Delete(metaDataId, userId);
        }
        
        public TakeDocModel.MetaDataFile Update(string fullName, byte[] data, Guid metadataId, Guid userId, TakeDocModel.Entity entity)
        {
            ICollection<TakeDocModel.MetaDataFile> mFiles = daoMdFile.GetBy(x => x.MetaDataId == metadataId && x.EntityId == entity.EntityId && x.EtatDeleteData == false);
            foreach (TakeDocModel.MetaDataFile mFile in mFiles) daoMdFile.Delete(mFile.MetaDataId, userId);

            string reference = daoMdFile.GenerateReference();

            System.IO.FileInfo f = this.GetFile(fullName);
            System.IO.FileInfo file = this.GenerateUNC(entity.EntityReference, TakeDocModel.Environnement.MetaDataFileStoreUNC, reference, f.Extension.Replace(".", string.Empty), true);
                                  
            TakeDocModel.MetaDataFile back = new TakeDocModel.MetaDataFile()
            {
                MetaDataFileReference = reference,
                MetaDataFilePath = file.FullName.Replace(TakeDocModel.Environnement.MetaDataFileStoreUNC, string.Empty),
                MetaDataFileName = string.Concat(reference,file.Extension),
                MetaDataId = metadataId,
                UserCreateData = userId,
                DateCreateData = System.DateTimeOffset.UtcNow,
                EtatDeleteData = false,
                EntityId = entity.EntityId,
                MetaDataFileExtension = file.Extension.Replace(".",string.Empty)
            };
            try
            {
                daoMdFile.Add(back);
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

        public string GetUrlFile(Guid metadataId, Guid entityId)
        {
            ICollection<TakeDocModel.MetaDataFile> files = daoMdFile.GetBy(x => x.MetaDataId == metadataId && x.EntityId == entityId && x.EtatDeleteData == false);
            byte[] data = System.IO.File.ReadAllBytes(string.Concat(TakeDocModel.Environnement.MetaDataFileStoreUNC, files.First().MetaDataFilePath));
            System.IO.FileInfo file = new System.IO.FileInfo(string.Concat(TakeDocModel.Environnement.TempDirectory, files.First().MetaDataFileName));
            if (System.IO.File.Exists(file.FullName)) System.IO.File.Delete(file.FullName);
            System.IO.File.WriteAllBytes(file.FullName, data);
            return file.Name;
        }
    }
}
