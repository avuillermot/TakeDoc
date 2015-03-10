using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TakeDocService.Document.Interface;
using TakeDocService.Document;
using Utility.MyUnityHelper;
using System.Collections.Generic;
using System.Linq;

namespace UnitTestTakeDocService.Document
{
    [TestClass]
    public class DocumentServiceTest : UnitTestTakeDocService.BaseServiceTest
    {
        IDocumentService servDocument = UnityHelper.Resolve<IDocumentService>();
        IVersionService servVersion = UnityHelper.Resolve<IVersionService>();
        IMetaDataService servMetaData = UnityHelper.Resolve<IMetaDataService>();
        IImageService servImage = new ImageService();
        TakeDocDataAccess.DaoBase<TakeDocModel.TypeDocument> daoTypeDocument = UnityHelper.Resolve<TakeDocDataAccess.DaoBase<TakeDocModel.TypeDocument>>();

        TakeDocModel.Document MyDocument
        {
            get
            {
                return UnitTestTakeDocService.TakeDocContext.CurrentDocument;
            }
            set
            {
                UnitTestTakeDocService.TakeDocContext.CurrentDocument = value;
            }
        }

        [TestInitialize]
        public void Init()
        {
            
        }

        [TestMethod]
        public void TestOrdered() {
            this.IsValidMetaData();

            this.CreateDocument();
            this.AddPage1();
            this.AddPage2();
            this.SetReceive();
            this.AddVersionMajor();
            this.AddPage1();
            this.SetReceive();
            this.AddVersionMinor();
            this.AddPage2();
            this.SetReceive();
        }
        
        [TestMethod]
        public void CreateDocument()
        {
            TakeDocModel.Environnement.Init(System.Configuration.ConfigurationManager.AppSettings);
            TakeDocModel.TypeDocument typeDocument = daoTypeDocument.GetBy(x => x.TypeDocumentReference == "NOTE_FRAIS").First();
            MyDocument = servDocument.Create(userId, entityId, typeDocument.TypeDocumentId, "Test creation document");

            Assert.IsTrue(MyDocument.Statut_Document.StatutDocumentReference.Equals(TakeDocModel.StatutDocument.Create), "Statut du document CREATE");
            Assert.IsTrue(MyDocument.Version.Count() == 1);
            Assert.IsTrue(MyDocument.LastVersion.VersionMajor);
            Assert.IsTrue(MyDocument.LastVersion.VersionNumber == 0);
            Assert.AreEqual(MyDocument.UserCreateData, userId);
            Assert.AreEqual(MyDocument.EntityId, entityId);
            Assert.IsNotNull(MyDocument.DateCreateData);
            Assert.AreNotEqual(MyDocument.DateCreateData, System.DateTimeOffset.MinValue);
            Assert.IsFalse(MyDocument.EtatDeleteData);
            Assert.IsFalse(MyDocument.LastVersion.EtatDeleteData);
            Assert.IsTrue(MyDocument.Type_Document.TypeDocumentReference == "NOTE_FRAIS");
            Assert.IsTrue(MyDocument.DocumentLabel == "Test creation document");
            Assert.IsTrue(MyDocument.LastVersionMetadata.Count() == 2);
        }

        [TestMethod]
        public void AddPage1()
        {
            string base64image = servImage.ToBase64String(TakeDocModel.Environnement.JpegTestFile1);
            servDocument.AddPage(userId, entityId, MyDocument.DocumentId, base64image, "jpeg",90);

            TakeDocModel.Version version = servVersion.GetById(MyDocument.LastVersion.VersionId, x => x.Page);

            Assert.AreEqual(version.LastPage.UserCreateData, userId);
            Assert.AreEqual(version.LastPage.EntityId, entityId);
            Assert.IsNotNull(version.LastPage.DateCreateData);
            Assert.IsNotNull(version.LastPage.PageNumber);
            Assert.AreNotEqual(version.LastPage.DateCreateData, System.DateTimeOffset.MinValue);
            Assert.IsFalse(version.LastPage.EtatDeleteData);
        }

        [TestMethod]
        public void AddPage2()
        {
            string base64image = servImage.ToBase64String(TakeDocModel.Environnement.JpegTestFile2);
            servDocument.AddPage(userId, entityId, MyDocument.DocumentId, base64image, "jpeg",180);

            TakeDocModel.Version version = servVersion.GetById(MyDocument.LastVersion.VersionId, x => x.Page);

            Assert.AreEqual(version.LastPage.UserCreateData, userId);
            Assert.AreEqual(version.LastPage.EntityId, entityId);
            Assert.IsNotNull(version.LastPage.DateCreateData);
            Assert.IsNotNull(version.LastPage.PageNumber);
            Assert.AreNotEqual(version.LastPage.DateCreateData, System.DateTimeOffset.MinValue);
            Assert.IsFalse(version.LastPage.EtatDeleteData);
        }

        [TestMethod]
        public void SetReceive()
        {
            servDocument.SetReceive(MyDocument.DocumentId);
            MyDocument = servDocument.GetById(MyDocument.DocumentId, x => x.Statut_Document);
            Assert.IsTrue(MyDocument.Statut_Document.StatutDocumentReference.Equals(TakeDocModel.StatutDocument.Complete), "Statut du document COMPLETE");
        }
        
        [TestMethod]
        public void AddVersionMajor()
        {
            servDocument.AddVersionMajor(userId, entityId, MyDocument.DocumentId);
            Assert.IsTrue(MyDocument.Version.Count() == 2);
            Assert.IsTrue(MyDocument.LastVersion.VersionMajor);
            Assert.IsTrue(MyDocument.LastVersion.VersionNumber == 1);
            Assert.IsTrue(MyDocument.LastVersionMetadata.Count() == 2);
        }

        [TestMethod]
        public void AddVersionMinor()
        {
            servDocument.AddVersionMinor(userId, entityId, MyDocument.DocumentId);
            Assert.IsTrue(MyDocument.Version.Count() == 3);
            Assert.IsTrue(MyDocument.LastVersion.VersionMajor == false);
            Assert.IsTrue(MyDocument.LastVersion.VersionNumber == 1.01M);
            Assert.IsTrue(MyDocument.LastVersionMetadata.Count() == 2);
        }

        [TestMethod]
        public void IsValidMetaData()
        {
            Assert.IsTrue(servMetaData.IsValid("System.String", "test",true));
            Assert.IsFalse(servMetaData.IsValid("System.DateTimeOffset", "test",true));
            Assert.IsTrue(servMetaData.IsValid("System.DateTimeOffset", System.DateTimeOffset.UtcNow.ToString(),true));
            Assert.IsFalse(servMetaData.IsValid("System.Boolean", "test",true));
            Assert.IsTrue(servMetaData.IsValid("System.Boolean", "true",true));
            Assert.IsFalse(servMetaData.IsValid("System.String", string.Empty, true));
            Assert.IsFalse(servMetaData.IsValid("System.String", null, true));
        }

        public void test()
        {
            byte[] img = System.IO.File.ReadAllBytes(@"d:\temp\page1.png");
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(new System.IO.MemoryStream(img));
            //servImage.DetectColorWithMarshal(bitmap, 0, 0, 0, 0);
        }
    }
}
