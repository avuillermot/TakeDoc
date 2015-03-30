using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocModel.Dto.Stats
{
    public class Dashboard
    {
        public Guid EntityId { get; set; }
        public string Type { get; set; }
        public string Code { get; set; }
        public int Value { get; set; }
    }
}
