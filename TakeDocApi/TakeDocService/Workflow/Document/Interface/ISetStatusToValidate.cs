﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocService.Workflow.Document.Interface
{
    public interface ISetStatusToValidate
    {
        void Execute(Guid userId);
    }
}