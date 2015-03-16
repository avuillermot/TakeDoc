using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocModel
{
    public partial class MetaData
    {
        public ICollection<DataFieldValue> DataFieldValue { get; set; }
    }
}
