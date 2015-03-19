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
            Utility.Logger.myLogger.Init();
            TakeDocModel.Environnement.Init(System.Configuration.ConfigurationManager.AppSettings);
            TakeDocService.Document.Interface.IDocumentService servDocument = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocService.Document.Interface.IDocumentService>();
            servDocument.GeneratePdf();
            Console.WriteLine("fin");
            //Console.Read();
        }
    }
}
