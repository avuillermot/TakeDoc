using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace UnitTestTakeDocService
{
    public class TakeDocContext
    {
        public static TakeDocModel.Document CurrentDocument { get; set; }   
    }

    [DeploymentItem("AppSettings.config")]
    public abstract class BaseServiceTest
    {
        public Guid userId = new Guid("A90CEA2D-7599-437B-88D3-A5405BE3EF93");
        public Guid entityId = new Guid("55C72E33-8864-4E0E-9BC8-C82378B2BF8C");

        private TestContext testContext;

        public TestContext TestContext
        {
            get { return testContext; }
            set { testContext = value; }
        }
    }
}
