﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocModel.Dto.Stats
{
    public class StatsDocument
    {
        public Guid EntityId { get; set; }
        public string EntityReference { get; set; }
        public Guid TypeDocumentId { get; set; }
        public string TypeDocumentReference { get; set; }
        public string TypeDocumentLabel { get; set; }
        public string StatusReference { get; set; }
        public string StatusLabel { get; set; }
        public int Count { get; set; }
    }
}
