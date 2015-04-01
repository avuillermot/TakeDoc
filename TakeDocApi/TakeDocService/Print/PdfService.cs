using System.Collections.Generic;
using System.Linq;
using Utility.MyUnityHelper;
using doc = TakeDocService.Document;
using iTextSharp.text.pdf;

namespace TakeDocService.Print
{
    public class PdfService : Interface.IPdfService
    {
        doc.Interface.IPageService servPage = UnityHelper.Resolve<doc.Interface.IPageService>();
        doc.Interface.IImageService servImage = UnityHelper.Resolve<doc.Interface.IImageService>();
        

        public PdfReader GetImagePdf(TakeDocModel.Version version) {
            ICollection<byte[]> data = new List<byte[]>();
            foreach (TakeDocModel.Page page in version.Page.OrderBy(x => x.PageNumber))
            {
                byte[] img = servPage.GetBinary(page.PageId);
                data.Add(img);
            }
            return servImage.GetPdf(data);
        }

        

        
    }
}
