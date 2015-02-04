using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using its = iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Drawing;

namespace TakeDocService.Document
{
    public class ImageService : BaseService, Interface.IImageService
    {
        public string PdfPath { get; set; }

        public ImageService()
        {
            this.PdfPath = TakeDocModel.Environnement.TempDirectory;
        }

        public byte[] GetPdfFromJpeg(ICollection<byte[]> pages)
        {
            string newFileName = System.Guid.NewGuid().ToString();
            string fileName = string.Concat(this.PdfPath, newFileName);
            using (its.Document doc = new its.Document(iTextSharp.text.PageSize.A4, -70, -70, 0, 0))
            {
                PdfWriter.GetInstance(doc, new FileStream(fileName, FileMode.Create));
                doc.Open();
                
                foreach (byte[] page in pages)
                {
                    iTextSharp.text.Image image = its.Image.GetInstance(page);
                    doc.NewPage();
                    PdfPTable table = new PdfPTable(1);
                    table.AddCell(new PdfPCell(image));
                    doc.Add(table);
                }
                doc.Close();
                doc.Dispose();
            }
            byte[] retour = System.IO.File.ReadAllBytes(fileName);
            if (System.IO.File.Exists(fileName)) System.IO.File.Delete(fileName);
            return retour;
        }

        public string ToBase64String(string pathFile)
        {
            using (Image image = Image.FromFile(pathFile))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();

                    // Convert byte[] to Base64 String
                    string base64String = Convert.ToBase64String(imageBytes);
                    return base64String;
                }
            }
        }

        public byte[] Rotate(Bitmap input)
        {
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                if (input.Width > input.Height)
                {
                    float angle = 90;
                    float sin = (float)Math.Abs(Math.Sin(angle * Math.PI / 180.0));

                    float originX = input.Height;
                    float originY = input.Width - (sin * input.Width);

                    Bitmap output = new Bitmap(input.Height, input.Width);
                    Graphics g = Graphics.FromImage(output);

                    g.TranslateTransform(originX, originY); // offset the origin to our calculated values
                    g.RotateTransform(angle); // set up rotate
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
                    g.DrawImageUnscaled(input, 0, 0); // draw the image at 0, 0
                    g.Dispose();

                    output.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    return ms.ToArray();
                }
                input.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }
    }
}
