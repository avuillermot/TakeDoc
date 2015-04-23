using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using its = iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Utility.MyUnityHelper;
using doc = TakeDocService.Document;


namespace TakeDocService.Document
{
    public class ImageService : BaseService, Interface.IImageService
    {
        doc.Interface.IPageService servPage = UnityHelper.Resolve<doc.Interface.IPageService>();

        public PdfReader GetImagePdf(TakeDocModel.Version version)
        {
            ICollection<byte[]> data = this.GetImage(version);
            return this.GetPdf(data);
        }

        public ICollection<byte[]> GetImage(TakeDocModel.Version version)
        {
            ICollection<byte[]> data = new List<byte[]>();
            foreach (TakeDocModel.Page page in version.Page.Where(x => x.EtatDeleteData == false).OrderBy(x => x.PageNumber))
            {
                byte[] img = servPage.GetBinary(page.PageId);
                data.Add(img);
            }
            return data;
        }

        private PdfReader GetPdf(ICollection<byte[]> pages)
        {
            MemoryStream streamOut = new MemoryStream();
            PdfReader reader = null;
            using (its.Document doc = new its.Document(iTextSharp.text.PageSize.A4, -70, -70, 0, 0))
            {
                PdfWriter.GetInstance(doc, streamOut);
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
                reader = new PdfReader(streamOut.ToArray());

                /*using (FileStream fs = File.Create(@"C:\temp\Test.pdf"))
                {
                    fs.Write(streamOut.ToArray(), 0, (int)streamOut.ToArray().Length);
                }
                reader = new PdfReader(@"C:\temp\Test.pdf");*/
            }
            return reader;
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

        private byte[] Rotate(Bitmap input, float angle)
        {
            if (angle != 0 && angle != 90 && angle != 180 && angle != 270 ) throw new Exception("Angle inconnu");
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                Bitmap output = new Bitmap(input);
                RotateFlipType transformation = RotateFlipType.RotateNoneFlipNone;
                if (angle == 90) transformation = RotateFlipType.Rotate90FlipNone;
                else if (angle == 180) transformation = RotateFlipType.Rotate180FlipNone;
                else if (angle == 270) transformation = RotateFlipType.Rotate270FlipNone;
                output.RotateFlip(transformation);
                output.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
        }
    }
}
