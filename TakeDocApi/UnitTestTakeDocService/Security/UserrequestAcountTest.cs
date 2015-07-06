using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TakeDocService.Workflow.Security.Interface;
using Utility.MyUnityHelper;
using System.Collections.Generic;
using System.Linq;

namespace UnitTestTakeDocService.Security
{
    [TestClass]
    public class UserRequestAcountTest : UnitTestTakeDocService.BaseServiceTest
    {
        IAccount servRequestAccount = UnityHelper.Resolve<IAccount>();
        TakeDocDataAccess.Security.Interface.IDaoUserTk daoUser = UnityHelper.Resolve<TakeDocDataAccess.Security.Interface.IDaoUserTk>();
 
        public void TestOrdered()
        {
            ICollection<TakeDocModel.UserTk> toDeletes1 = daoUser.GetBy(x => x.UserTkEmail == "a.vuillermot@hotmail.com");
            daoUser.Delete(toDeletes1);

            this.RequestAccountTest_create();
            this.RequestAccountTest_errorEmail();
            this.RequestAccountTest_errorPassword();

            ICollection<TakeDocModel.UserTk> toDeletes2 = daoUser.GetBy(x => x.UserTkEmail == "a.vuillermot@hotmail.com");
            daoUser.Delete(toDeletes2);
        }

        [TestMethod]
        public void RequestAccountTest_create()
        {
            bool ok = servRequestAccount.CreateRequest("alexandre", "vuillermot", "a.vuillermot@hotmail.com", "testpwd", "fr", "MASTER");
            Assert.IsTrue(ok, "Account should be created");
        }
        [TestMethod]
        public void RequestAccountTest_errorEmail()
        {
            bool ok = false;
            try
            {
                ok = servRequestAccount.CreateRequest("alexandre", "vuillermot", "a.vuillermot@hotmail.com", "testpwd", "fr", "MASTER");
            }
            catch(Exception ex) {
                // success
            }
            Assert.IsFalse(ok, "Email already exist");
        }
        [TestMethod]
        public void RequestAccountTest_errorPassword()
        {
            bool ok = false;
            try
            {
                ok = servRequestAccount.CreateRequest("alexandre", "vuillermot", "avuillermot@hotmail.com", "test", "fr", "MASTER");
            }
            catch (Exception ex)
            {
                // success
            }
            Assert.IsFalse(ok, "Password too short");
        }
    }
}
