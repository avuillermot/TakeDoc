using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocDataAccess.Document
{
    public class DaoMetaData : DaoBase<TakeDocModel.MetaData>, Interface.IDaoMetaData
    {
        public TakeDocModel.MetaData Add(TakeDocModel.MetaData meta)
        {
            if (meta.MetaDataId == System.Guid.Empty) meta.MetaDataId = System.Guid.NewGuid();

            this.Context.MetaData.Add(meta);
            this.Context.SaveChanges();

            return meta;
        }

        public void SetMetaData(Guid userId, Guid entityId, Guid versionId, ICollection<TakeDocModel.MetaData> metadatas)
        {
            foreach (TakeDocModel.MetaData metadata in metadatas)
            {
                metadata.MetaDataValue = metadata.MetaDataValue;
                metadata.UserUpdateData = userId;
                metadata.DateUpdateData = System.DateTimeOffset.UtcNow;
            }
            this.Update(metadatas);
        }
    }
}
