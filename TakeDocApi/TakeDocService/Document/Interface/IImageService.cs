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
        byte[] GetPdf(ICollection<byte[]> pages);
        string ToBase64String(string pathFile);
        byte[] Rotate(System.Drawing.Bitmap bitmap, float angle);
        void DetectColorWithMarshal(Bitmap image, byte searchedR, byte searchedG, int searchedB, int tolerance);
    }
}
