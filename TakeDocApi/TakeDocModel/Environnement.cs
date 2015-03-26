using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocModel
{
    public static class Environnement
    {
        public static void Init(System.Collections.Specialized.NameValueCollection settings)
        {
            VersionStoreUNC = settings["VersionStoreUNC"].ToString();
            PageStoreUNC = settings["PageStoreUNC"].ToString();
            TempDirectory = settings["TempDirectory"].ToString();
            JpegTestFile1 = settings["JpegTestFile1"].ToString();
            PdfTemp = settings["PdfTemp"].ToString();
        }
        public static string PageStoreUNC { get; set; }
        public static string VersionStoreUNC { get; set; }
        public static string TempDirectory { get; set; }
        public static string JpegTestFile1 { get; set; }
        public static string JpegTestFile2 { get; set; }
        public static string PdfTemp { get; set; }
    }
}
