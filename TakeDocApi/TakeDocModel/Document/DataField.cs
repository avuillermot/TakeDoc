using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocModel
{
    public partial class DataField
    {
        public bool IsAutocomplete
        {
            get
            {
                return this.DataFieldAutoCompleteId != null;
            }
            set
            {

            }
        }
        public bool IsList
        {
            get
            {
                return (this.DataFieldValue != null && this.DataFieldValue.Count() > 0);
            }
            set
            {

            }
        }
    }
}
