using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocService.Document.Interface
{
    public interface IDataFieldService
    {
        /// <summary>
        /// Return datafield for document type
        /// </summary>
        /// <param name="typeDocumentId"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        ICollection<TakeDocModel.View_TypeDocumentDataField> GetDataField(Guid typeDocumentId, Guid entityId);
    }
}
