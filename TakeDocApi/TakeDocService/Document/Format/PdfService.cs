using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.MyUnityHelper;
using System.IO;
using doc = TakeDocService.Document;

namespace TakeDocService.Document.Format
{
    public class PdfService : Interface.IPdfService
    {
        TakeDocDataAccess.Parameter.Interface.IDaoEntity daoEntity = UnityHelper.Resolve<TakeDocDataAccess.Parameter.Interface.IDaoEntity>();
        
        doc.Interface.IPageService servPage = UnityHelper.Resolve<doc.Interface.IPageService>();
        doc.Interface.IImageService servImage = UnityHelper.Resolve<doc.Interface.IImageService>();

        public byte[] GeneratePdf(TakeDocModel.Version version)
        {
            TakeDocModel.Entity entity = daoEntity.GetBy(x => x.EntityId == version.EntityId).First();

            ICollection<byte[]> data = new List<byte[]>();
            foreach (TakeDocModel.Page page in version.Page.OrderBy(x => x.PageNumber))
            {
                byte[] img = servPage.GetBinary(page.PageId);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(new System.IO.MemoryStream(img));
                float angle = 0;
                bool ok = float.TryParse(page.PageRotation.ToString(), out angle);
                if (ok) img = servImage.Rotate(bitmap, angle);
                data.Add(img);
            }
            byte[] back = servImage.GetPdf(data);
            return back;
        }
    }
}
