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
        public void Get()
        {
            ITypeDocumentService servTypeDocument = UnityHelper.Resolve<ITypeDocumentService>();
            ICollection<Type_Document> types = servTypeDocument.Get(userId, entityId);
            Assert.IsTrue(types.Count > 0);
        }
    }
}
