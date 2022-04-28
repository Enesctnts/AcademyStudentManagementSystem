using ASMSDataAccessLayer.ContactsDAL;
using ASMSEntityLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASMSDataAccessLayer.ImplementationsDAL
{
    public class CityRepo : RepositoryBase<City,byte>,ICityRepo
    {
        //ICityRepo kalıtım 


        public CityRepo(MyContext myContext):base(myContext)
        {
            // ctor oluşturmamızın sebebi kalıtım aldıgımız class'ın ctor'ında myContext istediği için
        }
    }
}
