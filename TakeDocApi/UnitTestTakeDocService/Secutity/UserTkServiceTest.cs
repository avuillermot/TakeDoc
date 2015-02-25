using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TakeDocService.Security.Interface;
using Utility.MyUnityHelper;
using System.Collections.Generic;

namespace UnitTestTakeDocService.Secutity
{
    [TestClass]
    public class UserTkServiceTest : UnitTestTakeDocService.BaseServiceTest
    {
        IUserTkService servUser = UnityHelper.Resolve<IUserTkService>();

        public void TestOrdered()
        {
            this.GetByLoginTest();
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
