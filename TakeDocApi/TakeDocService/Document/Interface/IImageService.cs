using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text.pdf;
using System.Drawing;

namespace TakeDocService.Document.Interface
{
    public interface IImageService
    {
        PdfReader GetImagePdf(TakeDocModel.Version version);
        string ToBase64String(string pathFile);
    }
}
