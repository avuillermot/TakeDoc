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
            Document.DocumentServiceTest documentTest = new Document.DocumentServiceTest();
            documentTest.TestOrdered();
            Console.WriteLine("fin");
            Console.Read();
        }
    }
}
