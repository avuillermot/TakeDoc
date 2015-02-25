using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestTakeDocService
{
    public class Program
    {
        public static void Main()
        {
            Utility.Logger.myLogger.Init();

            Document.DocumentServiceTest documentTest = new Document.DocumentServiceTest();
            Secutity.UserTkServiceTest userTest = new Secutity.UserTkServiceTest();
            //documentTest.TestOrdered();
            //userTest.TestOrdered();
            Console.WriteLine("fin");
            Console.Read();
        }


    }
}
