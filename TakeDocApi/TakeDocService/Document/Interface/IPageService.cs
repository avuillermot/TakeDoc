using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocService.Document.Interface
{
    public interface IPageService
    {
        void Add(Guid userId, Guid entityId, Guid versionId, string imageString, string extension);
        byte[] GetBinary(Guid pageId);
    }
}
