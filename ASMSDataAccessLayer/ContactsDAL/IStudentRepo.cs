using ASMSEntityLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASMSDataAccessLayer.ContactsDAL
{
    public interface IStudentRepo : IRepositoryBase<Student, string>
    {
    }
}
