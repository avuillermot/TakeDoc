using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestDataAccessTakeDoc
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            TakeDocDataAccess.Document.Interface.IDaoTypeDocument dao = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocDataAccess.Document.Interface.IDaoTypeDocument>();
            dao.Get(new System.Guid(), new System.Guid());
        }
    }
}
