using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TakeDocService.Workflow.Security.Interface;
using Utility.MyUnityHelper;
using System.Collections.Generic;
using System.Linq;

namespace UnitTestTakeDocService.Security
{
    [TestClass]
    public class TokenServiceTest : UnitTestTakeDocService.BaseServiceTest
    {
        TakeDocService.Security.Interface.ITokenService servToken = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocService.Security.Interface.ITokenService>();
        public void TestOrdered()
        {
            this.CreateTokenTest();
        }

        [TestMethod]
        public void CreateTokenTest()
        {
            var back = servToken.CreateRefreshToken(userId, "TEST");
            servToken.GetAccessToken(back.Id);
        }
    }
}
