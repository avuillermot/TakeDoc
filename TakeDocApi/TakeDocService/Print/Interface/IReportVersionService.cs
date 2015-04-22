using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocService.Print.Interface
{
    public interface IReportVersionService : IReportService<TakeDocModel.Version>
    {
        byte[] Generate(Guid versionId, Guid entity);
        byte[] GetBinaryFile(Guid versionId, Guid entityId);
        string GetUrlFile(Guid versionId, Guid entityId);
    }
}
