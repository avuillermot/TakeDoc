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
            if (string.IsNullOrEmpty(file.MetaDataFileReference)) file.MetaDataFileReference = base.GenerateReference("MetaDataFile");
            file.DateCreateData = System.DateTimeOffset.UtcNow;
            if (file.MetaDataFileId.Equals(System.Guid.Empty)) file.MetaDataFileId = System.Guid.NewGuid();
            this.Context.MetaDataFile.Add(file);
            this.Context.SaveChanges();

            return file;
        }

        public new void Delete(TakeDocModel.MetaDataFile file)
        {
            this.Context.MetaDataFile.Remove(file);
            this.Context.SaveChanges();
        }

        public string GenerateReference()
        {
            return this.Context.GenerateReference("MetaDataFile");
        }
    }
}
