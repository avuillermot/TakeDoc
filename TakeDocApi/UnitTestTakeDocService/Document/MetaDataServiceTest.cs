using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TakeDocService.Document.Interface;
using TakeDocService.Document;
using Utility.MyUnityHelper;
using System.Collections.Generic;
using System.Linq;

namespace UnitTestTakeDocService.Document
{
    /// <summary>
    /// Description résumée pour MetaDataServiceTest
    /// </summary>
    [TestClass]
    public class MetaDataServiceTest : UnitTestTakeDocService.BaseServiceTest
    {
        IDocumentService servDocument = UnityHelper.Resolve<IDocumentService>();
        IMetaDataService servMeta = UnityHelper.Resolve<IMetaDataService>();
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
        public void TestOrdered()
        {
            this.CreateDocumentTest();
            //this.GetMetadDataTest();

        }

        [TestMethod]
        public void CreateDocumentTest()
        {
            TakeDocModel.Environnement.Init(System.Configuration.ConfigurationManager.AppSettings);
            TakeDocModel.TypeDocument typeDocument = daoTypeDocument.GetBy(x => x.TypeDocumentReference == "NOTE_FRAIS").First();
            MyDocument = servDocument.Create(userId, entityId, typeDocument.TypeDocumentId, "Test creation document");

            string base64image = servImage.ToBase64String(TakeDocModel.Environnement.JpegTestFile1);
            servDocument.AddPage(userId, entityId, MyDocument.DocumentId, base64image, "jpeg",90);
        }

        [TestMethod]
        public void GetMetadDataTest()
        {
            ICollection<TakeDocModel.MetaData> metas = servMeta.GetByVersion(MyDocument.DocumentCurrentVersionId.Value, MyDocument.EntityId);
            Assert.IsTrue(metas.Count() > 0);
            TakeDocModel.MetaData mois = metas.Where(x => x.MetaDataName == "MONTH").First();
            Assert.IsTrue(mois.DataFieldValues.ToList().Count()  == 12);
        }


    }
}
