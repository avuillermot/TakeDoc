using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.MyUnityHelper;
using System.IO;
using doc = TakeDocService.Document;
using its = iTextSharp.text;
using iTextSharp.text.pdf;

namespace TakeDocService.Document.Format
{
    public class PdfService : Interface.IPdfService
    {
        doc.Interface.IPageService servPage = UnityHelper.Resolve<doc.Interface.IPageService>();
        doc.Interface.IImageService servImage = UnityHelper.Resolve<doc.Interface.IImageService>();

        private FileInfo GenerateStarterPdf(TakeDocModel.Version version, TakeDocModel.Entity entity)
        {
            FileInfo modele = new FileInfo(string.Concat(TakeDocModel.Environnement.ModelDirectory,entity.EntityReference,@"\",version.Document.Type_Document.TypeDocumentReference,"_","starter.odt"));
            FileInfo destinationOdt = new FileInfo(string.Concat(TakeDocModel.Environnement.TempDirectory,Guid.NewGuid().ToString(),modele.Extension));
            FileInfo destinationPdf = new FileInfo(string.Concat(destinationOdt.FullName.Replace(destinationOdt.Extension,".pdf")));

            modele.CopyTo(destinationOdt.FullName);

            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(TakeDocModel.Environnement.BatchConvertPdf);
            info.WorkingDirectory = TakeDocModel.Environnement.TempDirectory;
            info.Arguments = destinationOdt.FullName;
            System.Diagnostics.Process process = System.Diagnostics.Process.Start(info);
            process.WaitForExit();

            if (destinationOdt.Exists) destinationOdt.Delete();
            return destinationPdf;
        }

        public byte[] GeneratePdf(TakeDocModel.Version version, TakeDocModel.Entity entity)
        {
            FileInfo destinationPdf = this.GenerateStarterPdf(version, entity);
            using (its.Document doc = new its.Document(iTextSharp.text.PageSize.A4, -70, -70, 0, 0))
            {
                PdfWriter.GetInstance(doc, new FileStream(destinationPdf.FullName, FileMode.Append));
                doc.Open();
                
                foreach (TakeDocModel.Page page in version.Page.OrderBy(x => x.PageNumber))
                {
                    byte[] img = servPage.GetBinary(page.PageId);
                    System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(new System.IO.MemoryStream(img));
                    float angle = 0;
                    bool ok = float.TryParse(page.PageRotation.ToString(), out angle);
                    if (ok) img = servImage.Rotate(bitmap, angle);

                    doc.NewPage();
                    iTextSharp.text.Image image = its.Image.GetInstance(img);
                    PdfPTable table = new PdfPTable(1);
                    table.AddCell(new PdfPCell(image));
                    doc.Add(table);
                }
                doc.Close();
                doc.Dispose();
            }

            byte[] retour = System.IO.File.ReadAllBytes(destinationPdf.FullName);
            if (destinationPdf.Exists) destinationPdf.Delete();
            return retour;
        }
    }
}
