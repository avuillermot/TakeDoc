using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TakeDocService.Document.Interface;
using Utility.MyUnityHelper;
using System.Collections.Generic;
using TakeDocModel;
using System.Linq;

namespace UnitTestTakeDocService.Document
{
    [TestClass]
    public class TypeDocumentServiceTest : BaseServiceTest
    {
        [TestMethod]
        public void TestOrdered()
        {
            /*this.AddDataField();
            this.AddBackOfficeUser();
            this.DeleteBackOfficeUser();*/
            this.GetBackOfficeUser();
        }

        [TestMethod]
        public void Get()
        {
            ITypeDocumentService servTypeDocument = UnityHelper.Resolve<ITypeDocumentService>();
            ICollection<TypeDocument> types = servTypeDocument.Get(userId, entityId);
            Assert.IsTrue(types.Count > 0);
        }

        [TestMethod]
        public void AddDataField()
        {
            ITypeDocumentService servTypeDocument = UnityHelper.Resolve<ITypeDocumentService>();
            //servTypeDocument.AddDataField("TESTAVT", "DATE_NOTE_FRAIS", true, false, null, "MASTER", "ROOT");
        }

        [TestMethod]
        public void AddBackOfficeUser()
        {
            ITypeDocumentService servTypeDocument = UnityHelper.Resolve<ITypeDocumentService>();
            ICollection<TakeDocModel.TypeDocument> types = servTypeDocument.Get(userId, entityId);
            servTypeDocument.AddBackOfficeUser(userId, types.First().TypeDocumentId, entityId, userId);
        }

        [TestMethod]
        public void DeleteBackOfficeUser()
        {
            ITypeDocumentService servTypeDocument = UnityHelper.Resolve<ITypeDocumentService>();
            ICollection<TakeDocModel.TypeDocument> types = servTypeDocument.Get(userId, entityId);
            servTypeDocument.DeleteBackOfficeUser(userId, types.First().TypeDocumentId, entityId, userId);
        }

        [TestMethod]
        public void GetBackOfficeUser()
        {
            ITypeDocumentService servTypeDocument = UnityHelper.Resolve<ITypeDocumentService>();
            servTypeDocument.GetBackOfficeUser(new Guid("75E455BD-0717-4121-BFBB-1AF71DB4DB95"), new Guid("55C72E33-8864-4E0E-9BC8-C82378B2BF8C"));
        }
    }
}
