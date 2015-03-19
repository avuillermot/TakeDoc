using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocModel
{
    public partial class Status_Document
    {
        public static string Create { get { return "CREATE"; } }
        public static string Send { get { return "SEND"; } }
        public static string Complete { get { return "COMPLETE"; } }
    }
}
