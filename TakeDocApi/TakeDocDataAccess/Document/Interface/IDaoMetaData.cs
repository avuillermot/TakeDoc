using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocDataAccess.Document.Interface
{
    public interface IDaoMetaData
    {
        TakeDocModel.MetaData Add(TakeDocModel.MetaData meta);
        void SetMetaData(Guid userId, Guid entityId, Guid documentId, IDictionary<string, string> values);
    }
}
