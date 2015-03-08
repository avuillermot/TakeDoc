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

        public void SetMetaData(Guid userId, Guid entityId, Guid versionId, IDictionary<string, string> values)
        {
            ICollection<TakeDocModel.MetaData> metadatas = this.Context.MetaData.Where(x => x.MetaDataVersionId == versionId && x.EntityId == entityId && x.EtatDeleteData == false).ToList();
            foreach (TakeDocModel.MetaData metadata in metadatas)
            {
                ICollection<KeyValuePair<string,string>> kvs = values.Where(x => x.Key == metadata.MetaDataName).ToList();
                if (kvs.Count() > 0) {
                    metadata.MetaDataValue = kvs.First().Value;
                    metadata.UserUpdateData = userId;
                    metadata.DateUpdateData = System.DateTimeOffset.UtcNow;
                }
            }
            this.Update(metadatas);
        }
    }
}
