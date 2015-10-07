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
        TakeDocDataAccess.Parameter.Interface.IDaoEntity daoEntity = UnityHelper.Resolve<TakeDocDataAccess.Parameter.Interface.IDaoEntity>();

        IDaoPage daoPage = UnityHelper.Resolve<IDaoPage>();

        private void AddPage(Guid userId, Guid entityId, Guid versionId, byte[] data, string extension, int rotation, int pageNumber)
        {
            base.Logger.DebugFormat("AddPage: add page to version [{0}], extension : [{1}], rotation [{2}]", versionId, extension, rotation);
            TakeDocModel.Page page = daoPage.Add(userId, entityId, versionId, extension, rotation, pageNumber);

            TakeDocModel.Entity entity = daoEntity.GetBy(x => x.EntityId == entityId).First();

            //**********************************
            // Store page in original format
            //**********************************
            // generate full path filename
            System.IO.FileInfo file = this.GeneratePageUNC(entity.EntityReference, page.PageReference, extension);
            // write full path file name
            base.Logger.DebugFormat("AddPage:  write byte in file [{0}]", file.FullName);
            System.IO.File.WriteAllBytes(file.FullName, data);

            page.PagePath = file.FullName.Replace(TakeDocModel.Environnement.PageStoreUNC,string.Empty);

            daoPage.Update(page);
        }

        public void Add(Guid userId, Guid entityId, Guid versionId, string imageString, string extension, int rotation, int pageNumber)
        {
            this.Logger.Debug("PageService.Add(Guid userId, Guid entityId, Guid versionId, string imageString, string extension, int rotation)");
            imageString = imageString.Replace(string.Format("data:image/{0};base64,", extension), string.Empty);
            byte[] bytes = Convert.FromBase64String(imageString);
            this.AddPage(userId, entityId, versionId, bytes, extension, rotation, pageNumber);
        }

        private void Update(Guid pageId, int rotation, int pageNumber, bool delete, Guid userId, Guid entityId)
        {
            TakeDocModel.Page page = daoPage.GetBy(x => x.PageId == pageId).First();
            page.PageRotation = rotation;
            page.PageNumber = pageNumber;
            if (delete)
            {
                page.UserDeleteData = userId;
                page.DateDeleteData = System.DateTimeOffset.UtcNow;
                page.EtatDeleteData = true;
            }
            else
            {
                page.UserUpdateData = userId;
                page.DateUpdateData = System.DateTimeOffset.UtcNow;
            }
            daoPage.Update(page);
        }

        public void Update(Newtonsoft.Json.Linq.JArray pages, Guid userId, Guid versionId, Guid entityId)
        {
            foreach (Newtonsoft.Json.Linq.JObject page in pages.OrderBy(obj => obj["pageNumber"]))
            {
                Guid id = Guid.Empty;
                try
                {
                    id = new Guid(page.Value<string>("id"));
                }
                catch (Exception ex)
                {
                    id = Guid.Empty;
                }
                
                int rotation = page.Value<int>("rotation");
                int pageNumber = page.Value<int>("pageNumber");
                string action = page.Value<string>("action");
                if (string.IsNullOrEmpty(action) == false)
                {
                    bool delete = action.ToUpper().Equals("DELETE");
                    bool add = action.ToUpper().Equals("ADD");
                    bool update = action.ToUpper().Equals("UPDATE");
                    if (delete && id != Guid.Empty)
                    {
                        this.Update(id, rotation, pageNumber, delete, userId, entityId);
                    }
                    else if (add)
                    {
                        string extension = page.Value<string>("base64Image").Split(';')[0].Replace("data:image/", string.Empty);
                        this.Add(userId, entityId, versionId, page.Value<string>("base64Image"), extension, rotation, pageNumber);
                    }
                    else if (update && id != Guid.Empty)
                    {
                        this.Update(id, rotation, pageNumber, delete, userId, entityId);
                    }
                }
            }
        }

        private System.IO.FileInfo GeneratePageUNC(string entite, string fileName, string extension)
        {
            return this.GenerateUNC(entite, TakeDocModel.Environnement.PageStoreUNC, fileName, extension);
        }

        private System.IO.FileInfo GenerateVignetteUNC(string entite, string fileName, string extension)
        {
            return this.GenerateUNC(entite, TakeDocModel.Environnement.VignetteStoreUNC, fileName, extension);
        }
        
        private System.IO.FileInfo GenerateUNC(string entite, string store, string fileName, string extension)
        {
            string storeLocalPath = string.Concat(@"\", entite, @"\", extension);
            base.Logger.DebugFormat("GenerateUNC path in [{0}]", storeLocalPath);
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
            return new System.IO.FileInfo(string.Concat(store, @"\", storeLocalPath, @"\", fileName, ".", extension));
        }
                
        public byte[] GetBinary(Guid pageId)
        {
            TakeDocModel.Page page = daoPage.GetBy(x => x.PageId == pageId).First();
            return System.IO.File.ReadAllBytes(string.Concat(TakeDocModel.Environnement.PageStoreUNC,page.PagePath));
        }

        public string GetBase64(Guid pageId)
        {
            TakeDocModel.Page page = daoPage.GetBy(x => x.PageId == pageId).First();
            byte[] data = System.IO.File.ReadAllBytes(String.Concat(TakeDocModel.Environnement.PageStoreUNC,page.PagePath));

            string prefix = string.Format("data:image/{0};base64,", page.PageFileExtension);
 
            return string.Concat(prefix, Convert.ToBase64String(data));
        }
    }
}
