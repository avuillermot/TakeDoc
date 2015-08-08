﻿using System;
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
        
        private void AddPage(Guid userId, Guid entityId, Guid versionId, byte[] data, string extension, int rotation)
        {
            base.Logger.DebugFormat("AddPage: add page to version [{0}], extension : [{1}], rotation [{2}]", versionId, extension, rotation);
            TakeDocModel.Page page = daoPage.Add(userId, entityId, versionId, extension, rotation);

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

        public void Add(Guid userId, Guid entityId, Guid versionId, string imageString, string extension, int rotation)
        {
            this.Logger.Debug("PageService.Add(Guid userId, Guid entityId, Guid versionId, string imageString, string extension, int rotation)");
            imageString = imageString.Replace(string.Format("data:image/{0};base64,", extension), string.Empty);
            byte[] bytes = Convert.FromBase64String(imageString);
            this.AddPage(userId, entityId, versionId, bytes, extension, rotation);
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

        public void Update(Guid versionId, string json)
        {
            ICollection<TakeDocModel.Page> pages = daoPage.GetBy(x => x.PageVersionId == versionId);

            Newtonsoft.Json.Linq.JArray data = Newtonsoft.Json.Linq.JArray.Parse(json);
            foreach (Newtonsoft.Json.Linq.JObject obj in data)
            {
                Guid pageId = new Guid(obj.Value<string>("id"));
                int rotation = obj.Value<int>("rotation");
                ICollection<TakeDocModel.Page> myPages = pages.Where(x => x.PageId == pageId).ToList();
                if (myPages.Count() > 0)
                {
                    TakeDocModel.Page page = myPages.First();
                    page.PageRotation = rotation;
                    daoPage.Update(page);
                }
            }

        }
    }
}
