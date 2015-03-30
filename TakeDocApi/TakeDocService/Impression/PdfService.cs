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

namespace TakeDocService.Impression
{
    public class PdfService : Interface.IPdfService
    {
        doc.Interface.IPageService servPage = UnityHelper.Resolve<doc.Interface.IPageService>();
        doc.Interface.IImageService servImage = UnityHelper.Resolve<doc.Interface.IImageService>();
        TakeDocService.Document.MetaDataService servMetaData = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocService.Document.MetaDataService>();

        private PdfReader GetImagePdf(TakeDocModel.Version version) {
            ICollection<byte[]> data = new List<byte[]>();
            foreach (TakeDocModel.Page page in version.Page.OrderBy(x => x.PageNumber))
            {
                byte[] img = servPage.GetBinary(page.PageId);
                data.Add(img);
            }
            return servImage.GetPdf(data);
        }

        private void FillField(TakeDocModel.Version version, FileInfo destination)
        {
            ICollection<TakeDocModel.Dto.Document.ReadOnlyMetadata> roMetaDatas = servMetaData.GetReadOnlyMetaData(version);

            ULibre.Drivers.Interface.IDriver model = new ULibre.Drivers.Implementation.OdtDriver();
            model.Open(destination.FullName);
            foreach (TakeDocModel.Dto.Document.ReadOnlyMetadata ro in roMetaDatas.OrderBy(x => x.DisplayIndex))
            {
                ICollection<string> line = new List<string>();
                line.Add((string.IsNullOrEmpty(ro.Label) ? string.Empty : ro.Label));
                line.Add((string.IsNullOrEmpty(ro.Text) ? string.Empty : ro.Text));
                model.AddLine("TabMetadata", line.ToArray<string>());
            }
            model.RemoveEmptyLine("TabMetadata");
            model.Save();
        }

        private byte[] GenerateStarterPdf(TakeDocModel.Version version, TakeDocModel.Entity entity)
        {
            FileInfo modele = new FileInfo(string.Concat(TakeDocModel.Environnement.ModelDirectory,entity.EntityReference,@"\",version.Document.Type_Document.TypeDocumentReference,"_","starter.odt"));
            if (modele.Exists == false) modele = new FileInfo(string.Concat(TakeDocModel.Environnement.ModelDirectory, "empty.odt"));

            FileInfo destinationOdt = new FileInfo(string.Concat(TakeDocModel.Environnement.TempDirectory,Guid.NewGuid().ToString(),modele.Extension));
            FileInfo destinationPdf = new FileInfo(string.Concat(destinationOdt.FullName.Replace(destinationOdt.Extension,".pdf")));

            modele.CopyTo(destinationOdt.FullName);

            this.FillField(version, destinationOdt);

            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(TakeDocModel.Environnement.BatchConvertPdf);
            info.WorkingDirectory = TakeDocModel.Environnement.TempDirectory;
            info.Arguments = destinationOdt.FullName;
            System.Diagnostics.Process process = System.Diagnostics.Process.Start(info);
            process.WaitForExit();
                        
            byte[] data = System.IO.File.ReadAllBytes(destinationPdf.FullName);
            if (destinationOdt.Exists) destinationOdt.Delete();
            if (destinationPdf.Exists) destinationPdf.Delete();
            return data;
        }

        public byte[] GeneratePdf(TakeDocModel.Version version, TakeDocModel.Entity entity)
        {
            PdfReader entetePdf = new PdfReader(this.GenerateStarterPdf(version, entity));
            PdfReader imagePdf = this.GetImagePdf(version);
            its.Document outputPdf = new its.Document(iTextSharp.text.PageSize.A4, -70, -70, 0, 0);
            MemoryStream streamOut = new MemoryStream();

            using (PdfCopy writer = new PdfCopy(outputPdf, streamOut))
            {
                outputPdf.Open();
                PdfImportedPage currentPage = null;
                for(int p = 1; p <= entetePdf.NumberOfPages; p++) {
                    currentPage = writer.GetImportedPage(entetePdf, p);
                    writer.AddPage(currentPage);
                }
                for (int p = 1; p <= imagePdf.NumberOfPages; p++)
                {
                    currentPage = writer.GetImportedPage(imagePdf, p);
                    writer.AddPage(currentPage);
                }
                outputPdf.Close();
                outputPdf.Dispose();
            }
            return streamOut.ToArray();
        }
    }
}
