using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakeDocDataAccess.Document.Interface;
using TakeDocDataAccess.Document;
using Utility.MyUnityHelper;

namespace TakeDocService.Document
{
    public class PageService : BaseService, Interface.IPageService
    {
        TakeDocDataAccess.DaoBase<TakeDocModel.View_PageStoreLocator> dao = new TakeDocDataAccess.DaoBase<TakeDocModel.View_PageStoreLocator>();
        TakeDocDataAccess.Parameter.Interface.IDaoEntity daoEntity = UnityHelper.Resolve<TakeDocDataAccess.Parameter.Interface.IDaoEntity>();

        IDaoPage daoPage = UnityHelper.Resolve<IDaoPage>();
        
        private void AddPage(Guid userId, Guid entityId, Guid versionId, byte[] data, string extension, int rotation)
        {
            TakeDocModel.Page page = daoPage.Add(userId, entityId, versionId, rotation);

            TakeDocModel.Entity entity = daoEntity.GetBy(x => x.EntityId == entityId).First();

            // generate full path filename
            System.IO.FileInfo file = this.GenerateUNC(entity.EntityReference, page.PageReference, extension);
            // write full path file name
            System.IO.File.WriteAllBytes(file.FullName, data);

            ICollection<TakeDocModel.View_PageStoreLocator> locators = new List<TakeDocModel.View_PageStoreLocator>();
            locators = dao.GetBy(x => x.StreamLocator.ToUpper() == file.FullName.ToUpper());
            page.PageStreamId = locators.First().StreamId;

            daoPage.Update(page);
        }

        public void Add(Guid userId, Guid entityId, Guid versionId, string imageString, string extension, int rotation)
        {
            imageString = imageString.Replace("data:image/jpeg;base64,", string.Empty);
            byte[] bytes = Convert.FromBase64String(imageString);
            this.AddPage(userId, entityId, versionId, bytes, extension, rotation);
        }

        private System.IO.FileInfo GenerateUNC(string entite, string fileName, string extension)
        {
            string storeLocalPath = string.Concat(@"\", entite, @"\", extension);
            string[] arr = storeLocalPath.Split('\\');
            string deep = string.Empty;
            foreach (string s in arr)
            {
                if (string.IsNullOrEmpty(s) == false)
                {
                    deep = string.Concat(deep, @"\", s);
                    if (System.IO.Directory.Exists(deep) == false) System.IO.Directory.CreateDirectory(string.Concat(TakeDocModel.Environnement.PageStoreUNC, @"\", deep));
                }
            }
            return new System.IO.FileInfo(string.Concat(TakeDocModel.Environnement.PageStoreUNC, @"\", storeLocalPath, @"\", fileName, ".", extension));
        }
        
        public byte[] GetBinary(Guid pageId)
        {
            TakeDocModel.Page page = daoPage.GetBy(x => x.PageId == pageId).First();
            TakeDocModel.View_PageStoreLocator locator = dao.GetBy(x => x.StreamId == page.PageStreamId).First();
            return System.IO.File.ReadAllBytes(locator.StreamLocator);
        }
    }
}
