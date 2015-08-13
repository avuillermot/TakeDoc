using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocModel.Dto.Document
{
    public class DocumentComplete
    {
        public TakeDocModel.View_DocumentExtended Document { get; set; }
        public ICollection<TakeDocModel.MetaData> MetaDatas { get; set; }
        public ICollection<object> Pages { get; set; }
    }
}
