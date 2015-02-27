using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TakeDocService.Security.Interface;
using Utility.MyUnityHelper;
using System.Collections.Generic;
using System.Security.Claims;

namespace UnitTestTakeDocService.Secutity
{
    [TestClass]
    public class UserTkServiceTest : UnitTestTakeDocService.BaseServiceTest
    {
        IUserTkService servUser = UnityHelper.Resolve<IUserTkService>();

        public void TestOrdered()
        {
            this.GetByLoginTest();
            this.GetClaimsByLoginTest();
            this.GetAllTest();
        }

        [TestMethod]
        public void GetByLoginTest()
        {
            TakeDocModel.UserTk user = servUser.GetByLogin("eleonore");
            Assert.IsTrue(user.UserTkLogin == "eleonore");
            Assert.IsTrue(user.Entitys.Count > 0);
        }

        [TestMethod]
        public void GetClaimsByLoginTest()
        {
            ClaimsPrincipal cp = servUser.GetClaimsByLogin("eleonore");
            Assert.IsTrue(cp.FindFirst("Login").Value == "eleonore");
        }

        [TestMethod]
        public void GetAllTest()
        {
            ICollection<TakeDocModel.UserTk> users = servUser.GetAll();
            foreach (TakeDocModel.UserTk user in users)
            {
                Assert.IsTrue(user.UserTkLogin == "eleonore");
                Assert.IsTrue(user.Entitys.Count > 0);
            }
        }
    }
}
