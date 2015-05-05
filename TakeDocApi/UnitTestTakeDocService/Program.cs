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

            Document.DocumentServiceTest documentTest = new Document.DocumentServiceTest();
            Document.MetaDataServiceTest metaDataTest = new Document.MetaDataServiceTest();
            Security.UserRequestAcountTest requestAccountTest = new Security.UserRequestAcountTest();
            Document.TypeDocumentServiceTest typeDocumentTest = new Document.TypeDocumentServiceTest();

            /*documentTest.TestOrdered();
            metaDataTest.TestOrdered();
            requestAccountTest.TestOrdered();*/
            typeDocumentTest.TestOrdered();

            /*ULibre.Drivers.Interface.IDriver driver = new ULibre.Drivers.Implementation.OdtDriver();
            System.IO.File.Copy(@"D:\Projets\TakeDoc\Library\MASTER\Model\NOTE_DE_FRAIS_entete.odt", @"D:\Projets\TakeDoc\Library\test.odt",true);

            driver.Open(@"D:\Projets\TakeDoc\Library\test.odt");

            driver.FillField("EntityLabel", "AVT Corp");
            ICollection<string> line = new List<string>();
            line.Add("Montant");
            line.Add("501 euros");
            
            driver.AddLine("TabMetadata", line.ToArray<string>());
            driver.RemoveEmptyLine("TabMetadata");
            driver.Save();

            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(@"D:\Projets\TakeDoc\Library\convert.bat");
            info.WorkingDirectory = @"D:\Projets\TakeDoc\Library\";
            info.Arguments = "test.odt";
            System.Diagnostics.Process.Start(info);*/

            Console.WriteLine("End");

        }
    }
}
