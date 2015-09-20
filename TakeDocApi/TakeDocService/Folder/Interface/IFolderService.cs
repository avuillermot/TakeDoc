using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace TakeDocService.Folder.Interface
{
    public interface IFolderService
    {
        TakeDocModel.Folder Create(Newtonsoft.Json.Linq.JObject jfolder);
        void Delete(Guid folderId, Guid userId, Guid entityId);
        TakeDocModel.Folder Update(Newtonsoft.Json.Linq.JObject jfolder);
        ICollection<TakeDocModel.Folder> GetByPeriod(ICollection<Guid> agendas, DateTimeOffset start, DateTimeOffset end);
        ICollection<object> GetJsonByPeriod(ICollection<JObject> agendas, DateTimeOffset start, DateTimeOffset end);
    }
}
