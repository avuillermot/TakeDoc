using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocModel.Dto.Document
{
    public class ReadOnlyMetadata
    {
        public string Name { get; set; }
        public Guid EntityId {get; set;}
        public string Label { get; set; }
        public string Value { get; set; }
        public string Text { get; set; }
        public int DisplayIndex { get; set; }
        public string Type { get; set; }
    }
}
