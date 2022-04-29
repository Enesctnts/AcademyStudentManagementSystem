﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASMSEntityLayer.ResultModels
{
    public interface IResult
    {
         bool IsSuccess { get; set; }
         string Message { get; set; }
    }
}
