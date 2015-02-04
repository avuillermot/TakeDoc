using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text.pdf;

namespace TakeDocService.Document.Interface
{
    public interface IImageService
    {
        byte[] GetPdfFromJpeg(ICollection<byte[]> pages);
        string ToBase64String(string pathFile);
        byte[] Rotate(System.Drawing.Bitmap bitmap);
    }
}
