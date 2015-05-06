using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TakeDocService.Document.Interface;
using Utility.MyUnityHelper;
using System.Collections.Generic;
using TakeDocModel;

namespace UnitTestTakeDocService.Document
{
    [TestClass]
    public class TypeDocumentServiceTest : BaseServiceTest
    {
        [TestMethod]
        public void TestOrdered()
        {
            this.AddDataField();
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
            servTypeDocument.AddDataField("EMPTY", "DATE_NOTE_FRAIS", true, false, null, "MASTER", "ROOT");
        }
    }
}
