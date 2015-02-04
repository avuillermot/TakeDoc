using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocModel
{
    public partial class View_PageStoreLocator
    {
        public System.IO.FileInfo File
        {
            get
            {
                return new System.IO.FileInfo(this.StreamLocator);
            }
        }
    }
}
