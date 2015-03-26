using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakeDocService.Document.Interface;

namespace UnitTestTakeDocService
{
    public class Program
    {
        public static void Main()
        {
            Utility.Logger.myLogger.Init();
            TakeDocModel.Environnement.Init(System.Configuration.ConfigurationManager.AppSettings);
            IVersionService servVersion = Utility.MyUnityHelper.UnityHelper.Resolve<IVersionService>();
            string name = servVersion.GetUrlFile(new Guid("1B47DA21-8E42-4B21-A6E2-00058510BA7C"), new Guid("4A8D729B-A670-4441-A07C-21C9FA69F70F"));
            /*Document.DocumentServiceTest documentTest = new Document.DocumentServiceTest();
            Document.MetaDataServiceTest metaDataTest = new Document.MetaDataServiceTest();*/

            /*documentTest.TestOrdered();
            metaDataTest.TestOrdered();*/
            /*Secutity.UserTkServiceTest userTest = new Secutity.UserTkServiceTest();
            //documentTest.test();
            for (int i = 0; i < 1; i++)
            {
                Console.WriteLine(i);
                documentTest.TestOrdered();
            }
            userTest.TestOrdered();*/
            Console.WriteLine("fin");
            Console.Read();
        }
    }
}
