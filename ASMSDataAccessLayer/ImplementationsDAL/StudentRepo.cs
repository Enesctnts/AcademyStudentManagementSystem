using ASMSDataAccessLayer.ContactsDAL;
using ASMSEntityLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASMSDataAccessLayer.ImplementationsDAL
{
    public class StudentRepo : RepositoryBase<Student, string>, IStudentRepo
    {
        public StudentRepo(MyContext myContext) : base(myContext)
        {

        }
    
    }
}
