using ASMSDataAccessLayer.ContactsDAL;
using ASMSEntityLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASMSDataAccessLayer.ImplementationsDAL
{
    public class UsersAddressRepo : RepositoryBase<UsersAddress, int>, IUsersAddressRepo
    {
        public UsersAddressRepo(MyContext myContext) : base(myContext)
        {

        }
    
    }
}
