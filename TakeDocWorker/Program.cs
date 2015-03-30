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
            TakeDocService.Workflow.Document.Interface.ISetStatusSend servTask = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocService.Workflow.Document.Interface.ISetStatusSend>();
            servTask.Execute(new Guid("A90CEA2D-7599-437B-88D3-A5405BE3EF93"));
            Console.WriteLine("fin");
            //Console.Read();
        }
    }
}
