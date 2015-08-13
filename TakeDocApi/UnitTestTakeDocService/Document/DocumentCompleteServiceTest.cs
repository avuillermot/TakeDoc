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
    public class DocumentCompleteServiceTest : UnitTestTakeDocService.BaseServiceTest
    {
        IDocumentCompleteService servDocumentComplete = UnityHelper.Resolve<IDocumentCompleteService>();
        
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
        }
       
    }
}
