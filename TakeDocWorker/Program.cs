using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TakeDocWorker
{
    class Program
    {
        static void Main(string[] args)
        {
            TakeDocService.Document.Interface.IDocumentService servDocument = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocService.Document.Interface.IDocumentService>();
            Console.WriteLine("fin");
            Console.Read();
        }
    }
}
