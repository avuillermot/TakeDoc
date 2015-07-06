using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocModel
{
    public partial class UserTk
    {
        public Guid RefreshToken { get; set; }
        public Guid AccessToken { get; set; }
    }
}
