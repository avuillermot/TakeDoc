using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocDataAccess.Document
{
    public class DaoMetaDataFile : DaoBase<TakeDocModel.MetaDataFile>, Interface.IDaoMetaDataFile
    {
        public TakeDocModel.MetaDataFile Add(TakeDocModel.MetaDataFile file)
        {
            string mimeType = string.Empty;
            ICollection<string> results = this.Context.GetMediaType(file.MetaDataFileExtension.Replace(".", string.Empty)).ToList();
            if (results.Count() > 0) mimeType = results.First();

            if (string.IsNullOrEmpty(file.MetaDataFileReference)) file.MetaDataFileReference = base.GenerateReference("MetaDataFile");
            file.DateCreateData = System.DateTimeOffset.UtcNow;
            file.MetaDataFileMimeType = mimeType;
            if (file.MetaDataFileId.Equals(System.Guid.Empty)) file.MetaDataFileId = System.Guid.NewGuid();
            this.Context.MetaDataFile.Add(file);
            this.Context.SaveChanges();

            return file;
        }

        public void Update(TakeDocModel.MetaDataFile file, Guid userId)
        {
            ICollection<TakeDocModel.MetaDataFile> toDeletes = this.Context.MetaDataFile.Where(x => x.MetaDataFileId == file.MetaDataFileId).ToList();
            foreach (TakeDocModel.MetaDataFile toDel in toDeletes)
            {
                toDel.UserUpdateData = userId;
                toDel.DateUpdateData = System.DateTimeOffset.UtcNow;
            }
            this.Context.SaveChanges();
        }

        public void Delete(Guid metaDataId, Guid userId)
        {
            ICollection<TakeDocModel.MetaDataFile> toDeletes = this.Context.MetaDataFile.Where(x => x.MetaDataId == metaDataId).ToList();
            foreach (TakeDocModel.MetaDataFile toDel in toDeletes)
            {
                toDel.UserDeleteData = userId;
                toDel.DateDeleteData = System.DateTimeOffset.UtcNow;
                toDel.EtatDeleteData = true;
            }
            this.Context.SaveChanges();
        }

        public string GenerateReference()
        {
            return this.Context.GenerateReference("MetaDataFile");
        }
    }
}
