using ASMSEntityLayer.ResultModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASMSEntityLayer.ViewModels;

namespace ASMSBusinessLayer.ContractsBLL
{
    public interface IStudentBusinessEngine
    {
        IResult Add(StudentVM student);
    }
}
