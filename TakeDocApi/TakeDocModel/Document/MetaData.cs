using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocModel
{
    public partial class MetaData
    {
        public ICollection<DataFieldValue> DataFieldValues { get; set; }
        public DataFieldAutoComplete AutoComplete { get; set; }

        public string HtmlType
        {
            get
            {
                if (this.DataField.DataFieldValue.Count() > 0) return "list";
                else if (this.DataField.DataFieldAutoComplete.Count() > 0) return "autocomplete";
                return this.DataField.DataFieldType.DataFieldInputType;
            }
        }
    }
}
