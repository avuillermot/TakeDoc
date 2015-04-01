using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakeDocModel;

namespace TakeDocService.Print.Interface
{
    public interface IReportService<T>
    {
        byte[] Generate(T current, TakeDocModel.Entity entity);
        byte[] GetBinaryFile(Guid versionId, Guid entityId);
        string GetUrlFile(Guid versionId, Guid entityId);
    }
}
