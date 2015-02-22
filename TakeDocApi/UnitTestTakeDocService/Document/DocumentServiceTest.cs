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
            /*this.SetMetaDataOk();
            this.SetMetaDataError();*/
        }
        
        [TestMethod]
        public void CreateDocument()
        {
            TakeDocModel.Environnement.Init(System.Configuration.ConfigurationManager.AppSettings);
            TakeDocModel.TypeDocument typeDocument = daoTypeDocument.GetBy(x => x.TypeDocumentReference == "EMPTY").First();
            MyDocument = servDocument.Create(userId, entityId, typeDocument.TypeDocumentId, "Test creation document");

            Assert.IsTrue(MyDocument.Statut_Document.StatutDocumentReference.Equals("CREATE"), "Statut du document CREATE");
            Assert.IsTrue(MyDocument.Version.Count() == 1);
            Assert.IsTrue(MyDocument.LastVersion.VersionMajor);
            Assert.IsTrue(MyDocument.LastVersion.VersionNumber == 0);
            Assert.AreEqual(MyDocument.UserCreateData, userId);
            Assert.AreEqual(MyDocument.EntityId, entityId);
            Assert.IsNotNull(MyDocument.DateCreateData);
            Assert.AreNotEqual(MyDocument.DateCreateData, System.DateTimeOffset.MinValue);
            Assert.IsFalse(MyDocument.EtatDeleteData);
            Assert.IsFalse(MyDocument.LastVersion.EtatDeleteData);
            Assert.IsTrue(MyDocument.Type_Document.TypeDocumentReference == "EMPTY");
            Assert.IsTrue(MyDocument.DocumentLabel == "Test creation document");
            Assert.IsTrue(MyDocument.MetaData.Count() > 0);
        }

        [TestMethod]
        public void AddPage1()
        {
            string base64image = servImage.ToBase64String(TakeDocModel.Environnement.JpegTestFile1);
            servDocument.AddPage(userId, entityId, MyDocument.DocumentId, base64image, "jpeg");

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
            servDocument.AddPage(userId, entityId, MyDocument.DocumentId, base64image, "jpeg");

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
            Assert.IsTrue(MyDocument.Statut_Document.StatutDocumentReference.Equals("RECEIVE"), "Statut du document RECEIVE");
        }

        [TestMethod]
        public void SetMetaDataOk()
        {
            IDictionary<string, string> metadatas = new Dictionary<string, string>();
            metadatas.Add("YEAR", System.DateTimeOffset.UtcNow.ToString());
            metadatas.Add("COMMENT", "ceci est un test");
            servMetaData.SetMetaData(userId, MyDocument.EntityId, MyDocument.DocumentId, metadatas);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "La valeur spécifiée n'est pas compatible avec le type de données du champ.")]
        public void SetMetaDataError()
        {
            IDictionary<string, string> metadatas = new Dictionary<string, string>();
            metadatas.Add("YEAR", "error");
            servMetaData.SetMetaData(userId, MyDocument.EntityId, MyDocument.DocumentId, metadatas);
        }

        [TestMethod]
        public void AddVersionMajor()
        {
            servDocument.AddVersionMajor(userId, entityId, MyDocument.DocumentId);
            Assert.IsTrue(MyDocument.Version.Count() == 2);
            Assert.IsTrue(MyDocument.LastVersion.VersionMajor);
            Assert.IsTrue(MyDocument.LastVersion.VersionNumber == 1);
        }

        [TestMethod]
        public void AddVersionMinor()
        {
            servDocument.AddVersionMinor(userId, entityId, MyDocument.DocumentId);
            Assert.IsTrue(MyDocument.Version.Count() == 3);
            Assert.IsTrue(MyDocument.LastVersion.VersionMajor == false);
            Assert.IsTrue(MyDocument.LastVersion.VersionNumber == 1.01M);
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
    }
}
