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
        public static string Incomplete { get { return "INCOMPLETE"; } }
        public static string Complete { get { return "COMPLETE"; } }
        public static string ToValidate { get { return "TO_VALIDATE"; } }
        public static string Approve { get { return "APPROVE"; } }
        public static string Refuse { get { return "REFUSE"; } }
        public static string Archive { get { return "ARCHIVE"; } }
    }
}
