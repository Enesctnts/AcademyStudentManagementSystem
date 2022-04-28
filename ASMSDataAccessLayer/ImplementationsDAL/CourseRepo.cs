using ASMSDataAccessLayer.ContactsDAL;
using ASMSEntityLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASMSDataAccessLayer.ImplementationsDAL
{
    public class CourseRepo:RepositoryBase<Course,int>,ICourseRepo
    {
        public CourseRepo(MyContext myContext) : base(myContext)
        {

        }
    }
}
