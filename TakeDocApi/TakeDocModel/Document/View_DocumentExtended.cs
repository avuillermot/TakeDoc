using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocModel
{
    public partial class View_DocumentExtended
    {
        [System.ComponentModel.DataAnnotations.Key]
        public System.Guid Id
        {
            get
            {
                return this.DocumentId;
            }
        }
    }
}
