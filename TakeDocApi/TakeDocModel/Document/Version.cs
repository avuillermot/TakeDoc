using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocModel
{
    public partial class Version
    {
        public TakeDocModel.Page LastPage
        {
            get
            {
                if (this.Page == null || this.Page.Count() == 0) return null;
                return this.Page.Where(x => x.EtatDeleteData == false).OrderByDescending(x => x.PageNumber).First();
            }
        }
    }
}
