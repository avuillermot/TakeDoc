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
    public class WorkflowAutoTest : UnitTestTakeDocService.BaseServiceTest
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
            TakeDocModel.TypeDocument typeDocument = daoTypeDocument.GetBy(x => x.TypeDocumentReference == "TEST-INTEGRATION-AUTO-VALIDATION").First();
            MyDocument = servDocument.Create(userId, entityId, typeDocument.TypeDocumentId, "Test intégration auto validation");

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
            Assert.IsTrue(MyDocument.Type_Document.TypeDocumentReference == "TEST-INTEGRATION-AUTO-VALIDATION");
            Assert.IsTrue(MyDocument.DocumentLabel == "Test intégration auto validation");
            Assert.IsTrue(MyDocument.LastVersionMetadata.Count() == 2);
            Assert.IsTrue(MyDocument.LastVersionMetadata.Any(x => x.MetaDataName == "REFACTURABLE") == true);
            Assert.IsTrue(MyDocument.LastVersionMetadata.Any(x => x.MetaDataName == "MONTANT") == true);
            ICollection<TakeDocModel.DocumentStatusHisto> histos = servStatus.GetStatus(MyDocument);
            Assert.IsTrue(histos.Count() == 0, "no histo for the moment");
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
            ICollection<TakeDocModel.Status_Document> status = servStatus.GetStatus(MyDocument.EntityId);

            IDictionary<string, object> metas = new Dictionary<string, object>();
            metas.Add("REFACTURABLE", "true");
            metas.Add("MONTANT", "20");
            throw new Exception("new metada");
            //servDocument.Update(userId, entityId, MyDocument.DocumentCurrentVersionId.Value, null, true);

            ICollection<TakeDocModel.DocumentStatusHisto> histos = servStatus.GetStatus(MyDocument);
            Assert.IsTrue(histos.Count() == 3, "create, complete, approve in this order");
            Assert.IsTrue(histos.ToArray()[0].DocumentStatusId == status.First(x => x.StatusDocumentReference == TakeDocModel.Status_Document.Create).StatusDocumentId, "first must be create");
            Assert.IsTrue(histos.ToArray()[1].DocumentStatusId == status.First(x => x.StatusDocumentReference == TakeDocModel.Status_Document.Complete).StatusDocumentId, "second must be complete");
            Assert.IsTrue(histos.ToArray()[2].DocumentStatusId == status.First(x => x.StatusDocumentReference == TakeDocModel.Status_Document.Approve).StatusDocumentId, "third must be approve");
            Assert.IsTrue(MyDocument.Status_Document.StatusDocumentReference == TakeDocModel.Status_Document.Archive, "must be archive");
            Assert.IsTrue(MyDocument.LastVersion.Status_Version.StatusVersionReference == TakeDocModel.Status_Version.Archive);
        }
    }
}
