using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace TakeDocDataAccess.Document.Interface
{
    public interface IDaoMetaDataFile
    {
        TakeDocModel.MetaDataFile Add(TakeDocModel.MetaDataFile file);
        void Delete(TakeDocModel.MetaDataFile file);
        ICollection<TakeDocModel.MetaDataFile> GetBy(Expression<Func<TakeDocModel.MetaDataFile, bool>> where, params Expression<Func<TakeDocModel.MetaDataFile, object>>[] properties);
        string GenerateReference();
    }
}
