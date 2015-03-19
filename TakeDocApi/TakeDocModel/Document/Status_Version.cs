using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocModel
{
    public partial class Status_Version
    {
        public static string Create { get { return "CREATE"; } }
        public static string DataSend { get { return "DATA_SEND"; } }
        public static string MetaSend { get { return "META_SEND"; } }
        public static string Complete { get { return "COMPLETE"; } }
    }
}
