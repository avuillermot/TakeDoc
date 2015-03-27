using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TakeDocService.Document.Format.Interface
{
    public interface IPdfService
    {
        byte[] GeneratePdf(TakeDocModel.Version version);
    }
}
