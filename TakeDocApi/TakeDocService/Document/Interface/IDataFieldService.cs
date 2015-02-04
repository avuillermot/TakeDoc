using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocService.Document.Interface
{
    public interface IDataFieldService
    {
        ICollection<TakeDocModel.DataField> GetDataField(Guid typeDocumentId, Guid entityId);
        ICollection<TakeDocModel.DataField> GetDataField(ICollection<string> fields, Guid entityId);
    }
}
