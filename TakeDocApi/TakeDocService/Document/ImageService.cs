﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using its = iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace TakeDocService.Document
{
    public class ImageService : BaseService, Interface.IImageService
    {
        public string PdfPath { get; set; }

        public ImageService()
        {
            this.PdfPath = TakeDocModel.Environnement.TempDirectory;
        }

        public void DetectColorWithMarshal(Bitmap image, byte searchedR, byte searchedG, int searchedB, int tolerance)
        {
            BitmapData imageData = image.LockBits(new Rectangle(0, 0, image.Width,
              image.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            byte[] imageBytes = new byte[Math.Abs(imageData.Stride) * image.Height];
            IntPtr scan0 = imageData.Scan0;

            Marshal.Copy(scan0, imageBytes, 0, imageBytes.Length);

            byte unmatchingValue = 0;
            byte matchingValue = 255;
            int toleranceSquared = tolerance * tolerance;

            for (int i = 0; i < imageBytes.Length; i += 3)
            {
                byte pixelB = imageBytes[i];
                byte pixelR = imageBytes[i + 2];
                byte pixelG = imageBytes[i + 1];

                int diffR = pixelR - searchedR;
                int diffG = pixelG - searchedG;
                int diffB = pixelB - searchedB;

                int distance = diffR * diffR + diffG * diffG + diffB * diffB;

                imageBytes[i] = imageBytes[i + 1] = imageBytes[i + 2] = distance >
                  toleranceSquared ? unmatchingValue : matchingValue;
            }

            Marshal.Copy(imageBytes, 0, scan0, imageBytes.Length);

            image.UnlockBits(imageData);
            if (System.IO.File.Exists(@"d:\temp\test.png")) System.IO.File.Delete(@"d:\temp\test.png");
            image.Save(@"d:\temp\test.png");
        }

        public byte[] GetPdf(ICollection<byte[]> pages)
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

        public byte[] Rotate(Bitmap input, float angle)
        {
            if (angle != 0 && angle != 90 && angle != 180 && angle != 270 ) throw new Exception("Angle inconnu");
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                RotateFlipType transformation = RotateFlipType.RotateNoneFlipNone;
                if (angle == 90) transformation = RotateFlipType.Rotate90FlipNone;
                else if (angle == 180) transformation = RotateFlipType.Rotate180FlipNone;
                else if (angle == 270) transformation = RotateFlipType.Rotate270FlipNone;

                input.RotateFlip(transformation);
                input.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }
    }
}
