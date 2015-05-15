using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TakeDocService.Document.Interface;
using TakeDocService.Document;
using TakeDocService.Print.Interface;
using Utility.MyUnityHelper;
using System.Collections.Generic;
using System.Linq;

namespace UnitTestTakeDocService.Workflow
{
    [TestClass]
    public class WorkflowNoTest : UnitTestTakeDocService.BaseServiceTest
    {
        TakeDocService.Workflow.Document.Interface.IStatus servStatus = new TakeDocService.Workflow.Document.Status();
        IDocumentService servDocument = UnityHelper.Resolve<IDocumentService>();
        IVersionService servVersion = UnityHelper.Resolve<IVersionService>();
        IImageService servImage = UnityHelper.Resolve<IImageService>();
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
            this.SetMetaData();
        }
        
        [TestMethod]
        public void CreateDocument()
        {
            TakeDocModel.Environnement.Init(System.Configuration.ConfigurationManager.AppSettings);
            TakeDocModel.TypeDocument typeDocument = daoTypeDocument.GetBy(x => x.TypeDocumentReference == "TEST-INTEGRATION-NO-VALIDATION").First();
            MyDocument = servDocument.Create(userId, entityId, typeDocument.TypeDocumentId, "Test intégration no validation");

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
            Assert.IsTrue(MyDocument.Type_Document.TypeDocumentReference == "TEST-INTEGRATION-NO-VALIDATION");
            Assert.IsTrue(MyDocument.DocumentLabel == "Test intégration no validation");
            Assert.IsTrue(MyDocument.LastVersionMetadata.Count() == 2);
            Assert.IsTrue(MyDocument.LastVersionMetadata.Any(x => x.MetaDataName == "REFACTURABLE") == true);
            Assert.IsTrue(MyDocument.LastVersionMetadata.Any(x => x.MetaDataName == "MONTANT") == true);
            // TODO : test  histo is equal 0
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

        public void SetMetaData()
        {
            IDictionary<string, string> metas = new Dictionary<string, string>();
            metas.Add("REFACTURABLE", "true");
            metas.Add("MONTANT", "20");
            servDocument.SetMetaData(userId, entityId, MyDocument.DocumentCurrentVersionId.Value, metas);

            Assert.IsTrue(MyDocument.LastVersion.Status_Version.StatusVersionReference == TakeDocModel.Status_Version.Archive);
            Assert.IsTrue(MyDocument.Status_Document.StatusDocumentReference == TakeDocModel.Status_Document.Archive);
        }
    }
}
