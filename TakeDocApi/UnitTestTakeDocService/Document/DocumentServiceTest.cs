using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TakeDocService.Document.Interface;
using TakeDocService.Document;
using TakeDocService.Print.Interface;
using Utility.MyUnityHelper;
using System.Collections.Generic;
using System.Linq;

namespace UnitTestTakeDocService.Document
{
    [TestClass]
    public class DocumentServiceTest : UnitTestTakeDocService.BaseServiceTest
    {
        TakeDocService.Workflow.Document.Interface.IStatus servStatus = new TakeDocService.Workflow.Document.Status();
        IDocumentService servDocument = UnityHelper.Resolve<IDocumentService>();
        IVersionService servVersion = UnityHelper.Resolve<IVersionService>();
        IMetaDataService servMetaData = UnityHelper.Resolve<IMetaDataService>();
        IImageService servImage = UnityHelper.Resolve<IImageService>();
        IReportVersionService servReport = UnityHelper.Resolve<IReportVersionService>();
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
            this.SearchTest();
            this.DeleteDocument();
            this.IsValidMetaData();

            this.CreateDocument();
            this.AddPage1();
            this.AddPage2();
            this.SetStatusIncomplete();
            this.SetStatusComplete();
            this.AddVersionMajor();
            this.AddPage1();
            this.SetStatusIncomplete();
            this.SetStatusComplete();
            this.AddVersionMinor();
            this.AddPage1();
            this.AddPage2();
            this.SetStatusIncomplete();
            this.SetStatusComplete();
        }

         [TestMethod]
        public void SearchTest()
        {
            Guid typeDoc = new Guid("A78A28CF-9D5C-421D-B008-72E070BAF9D6");
            Guid entityId = new Guid("4A8D729B-A670-4441-A07C-21C9FA69F70F");

             ICollection<TakeDocModel.Dto.Document.SearchMetadata> metas = new List<TakeDocModel.Dto.Document.SearchMetadata>();
             metas.Add(new TakeDocModel.Dto.Document.SearchMetadata(TakeDocModel.Dto.Document.SearchCondition.Start)
             {
                 MetaDataName = "DEVISE",
                 MetaDataValue = "DOLLARS"
             });

             /*metas.Add(new TakeDocModel.MetaData()
             {
                 MetaDataName = "MONTANT",
                 MetaDataValue = "5"
             });*/

             ICollection<TakeDocModel.View_DocumentExtended> docs = servDocument.Search(typeDoc, metas, userId, entityId);
        }
        
        [TestMethod]
        public void CreateDocument()
        {
            TakeDocModel.Environnement.Init(System.Configuration.ConfigurationManager.AppSettings);
            TakeDocModel.TypeDocument typeDocument = daoTypeDocument.GetBy(x => x.TypeDocumentReference == "NOTE_FRAIS").First();
            MyDocument = servDocument.Create(userId, entityId, typeDocument.TypeDocumentId, "Test creation document");

            Assert.IsTrue(MyDocument.Status_Document.StatusDocumentReference.Equals(TakeDocModel.Status_Document.Create), "Statut du document CREATE");
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
            Assert.IsTrue(MyDocument.LastVersionMetadata.Count() > 2);
        }

        [TestMethod]
        public void AddPage1()
        {
            string base64image = servImage.ToBase64String(TakeDocModel.Environnement.JpegTestFile1);
            servDocument.AddPage(userId, entityId, MyDocument.DocumentId, base64image, "jpeg",0);

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
        public void SetStatusIncomplete()
        {
            servStatus.SetStatus(MyDocument.DocumentId, TakeDocModel.Status_Document.Incomplete, userId, true);
            MyDocument = servDocument.GetById(MyDocument.DocumentId, x => x.Status_Document);
            Assert.IsTrue(MyDocument.Status_Document.StatusDocumentReference.Equals(TakeDocModel.Status_Document.Incomplete), "Statut du document DataSend");
        }

        [TestMethod]
        public void SetStatusComplete()
        {
            servStatus.SetStatus(MyDocument.DocumentId, TakeDocModel.Status_Document.Complete, userId, true);
            MyDocument = servDocument.GetById(MyDocument.DocumentId, x => x.Status_Document);
            Assert.IsTrue(MyDocument.Status_Document.StatusDocumentReference.Equals(TakeDocModel.Status_Document.Complete), "Statut du document MetaSend");
        }
        
        [TestMethod]
        public void AddVersionMajor()
        {
            servDocument.AddVersionMajor(userId, entityId, MyDocument.DocumentId);
            Assert.IsTrue(MyDocument.Version.Count() == 2);
            Assert.IsTrue(MyDocument.LastVersion.VersionMajor);
            Assert.IsTrue(MyDocument.LastVersion.VersionNumber == 1);
            Assert.IsTrue(MyDocument.LastVersionMetadata.Count() > 2);
        }

        [TestMethod]
        public void AddVersionMinor()
        {
            servDocument.AddVersionMinor(userId, entityId, MyDocument.DocumentId);
            Assert.IsTrue(MyDocument.Version.Count() == 3);
            Assert.IsTrue(MyDocument.LastVersion.VersionMajor == false);
            Assert.IsTrue(MyDocument.LastVersion.VersionNumber == 1.01M);
            Assert.IsTrue(MyDocument.LastVersionMetadata.Count() > 2);
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

        [TestMethod]
        public void DeleteDocument()
        {
            //servDocument.Delete(new Guid("03679883-0437-4AED-AFE9-82CCB43F141F"), entityId, userId);
        }

        public void test()
        {
            byte[] img = System.IO.File.ReadAllBytes(@"d:\temp\page1.png");
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(new System.IO.MemoryStream(img));
            //servImage.DetectColorWithMarshal(bitmap, 0, 0, 0, 0);
        }
    }
}
