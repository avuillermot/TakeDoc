using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TakeDocService.Document.Interface;
using Utility.MyUnityHelper;
using System.Collections.Generic;
using TakeDocModel;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace UnitTestTakeDocService.Folder
{
    [TestClass]
    public class FolderServiceTest : BaseServiceTest
    {
        TakeDocService.Folder.Interface.IFolderService servFolder = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocService.Folder.Interface.IFolderService>();

        [TestMethod]
        public void TestOrdered()
        {
            this.AddFolder();
        }

        [TestMethod]
        public void AddFolder()
        {
            string json = "entityId: \"{0}\", userCreateId: \"{1}\", ownerId: \"{2}\", label: \"{3}\", folderTypId: \"{4}\", dateStart: \"{5}\", dateEnd: \"{6}\"";
            string value = string.Concat("{", string.Format(json, base.entityId.ToString() , base.userId.ToString() , base.userId.ToString(), "mon rdv", "FAC8EFBC-001D-4C4B-85EF-8ACDDE1EA724","2015-09-03T20:07:39.560Z","2015-09-03T21:07:39.560Z"), "}");

            JObject jfolder = JObject.Parse(value);
            servFolder.Create(jfolder);
        }
    }
}
