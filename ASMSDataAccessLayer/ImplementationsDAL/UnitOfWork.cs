using ASMSDataAccessLayer.ContactsDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASMSDataAccessLayer.ImplementationsDAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MyContext _myContext;
        public UnitOfWork(MyContext myContext)
        {
            _myContext = myContext;
            CityRepo = new CityRepo(_myContext);
            ClassRepo = new ClassRepo(_myContext);
            DistrictRepo = new DistrictRepo(_myContext);
            NeighbourhoodRepo = new NeighbourhoodRepo(_myContext);
            StudentRepo = new StudentRepo(_myContext);
            StudentsCourseGroupRepo = new StudentsCourseGroupRepo(_myContext);
            CourseGroupRepo = new CourseGroupRepo(_myContext);
            CourseRepo = new CourseRepo(_myContext);
            TeacherRepo = new TeacherRepo(_myContext);
            UsersAddressRepo = new UsersAddressRepo(_myContext);
            StudentAttendanceRepo = new StudentAttendanceRepo(_myContext);
        }
        public ICityRepo CityRepo {get;}

        public IClassRepo ClassRepo {get;}

        public ICourseGroupRepo CourseGroupRepo {get;}

        public ICourseRepo CourseRepo {get;}

        public IDistrictRepo DistrictRepo {get;}

        public INeighbourhoodRepo NeighbourhoodRepo {get;}

        public IStudentAttendanceRepo StudentAttendanceRepo {get;}

        public IStudentRepo StudentRepo {get;}

        public IStudentsCourseGroupRepo StudentsCourseGroupRepo {get;}

        public ITeacherRepo TeacherRepo {get;}

        public IUsersAddressRepo UsersAddressRepo {get;}


    }
}
