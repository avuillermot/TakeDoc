using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocModel
{
    public partial class StatutVersion
    {
        public static string Create { get { return "CREATE"; } }
        public static string NoMeta { get { return "NO_META"; } }
        public static string Complete { get { return "COMPLETE"; } }
    }
}
