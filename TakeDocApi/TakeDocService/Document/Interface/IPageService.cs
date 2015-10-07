using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocService.Document.Interface
{
    public interface IPageService
    {
        void Add(Guid userId, Guid entityId, Guid versionId, string imageString, string extension, int rotation, int pageNumber);
        byte[] GetBinary(Guid pageId);
        string GetBase64(Guid pageId);
        void Update(Newtonsoft.Json.Linq.JArray pages, Guid userId, Guid versionId, Guid entityId);
    }
}
