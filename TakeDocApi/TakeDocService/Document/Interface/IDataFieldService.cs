using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocService.Document.Interface
{
    public interface IDataFieldService
    {
        ICollection<TakeDocModel.View_TypeDocumentDataField> GetDataField(Guid typeDocumentId, Guid entityId);
        ICollection<TakeDocModel.View_TypeDocumentDataField> GetDataField(ICollection<string> fields, Guid entityId);
    }
}
