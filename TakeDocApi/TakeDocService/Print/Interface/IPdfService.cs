using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using iTextSharp.text.pdf;

namespace TakeDocService.Print.Interface
{
    public interface IPdfService
    {
        /// <summary>
        /// Return picture of version in pdf
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        PdfReader GetImagePdf(TakeDocModel.Version version);
    }
}
