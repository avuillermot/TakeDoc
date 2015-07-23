using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocService.Document.Interface
{
    public interface IMetaDataFileService
    {
        TakeDocModel.MetaDataFile Create(string path, byte[] data, Guid metadataId, Guid userId, TakeDocModel.Entity entity);
        System.IO.FileInfo GetFile(string fullName);
        string GetUrlFile(Guid metadataId, Guid entityId);
    }
}
