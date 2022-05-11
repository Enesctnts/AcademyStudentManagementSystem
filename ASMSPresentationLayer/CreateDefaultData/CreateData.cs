using ASMSEntityLayer.Enums;
using ASMSEntityLayer.IdentityModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASMSPresentationLayer.CreateDefaultData
{
    public class CreateData
    {
        //Burayı yapmamızın sebebi proje ayaga kaltıgı zaman rolleri veri tabanına yazmamızı sağlıyor
        public static void Create(RoleManager<AppRole> roleManager)
        {
            CheckAndCreateRoles(roleManager);
        }

        private static void CheckAndCreateRoles(RoleManager<AppRole> roleManager)
        {
            var allRoles = Enum.GetNames(typeof(ASMSRoles));

            foreach (var item in allRoles)
            {

                if (!roleManager.RoleExistsAsync(item).Result)
                {

                    AppRole role = new AppRole()
                    {
                        Name = item,
                        IsDeleted = false,
                        CreatedDate=DateTime.Now,
                        Description=$"Sistem tarafından {item} rolü ektedir."
                    };
                    //Asenkron işlem yaptıgımız için roleManager.CreateAsync(role).Result sonuna result yapmamız lazım rol ataması yaparken bu işlemleri hepsini yapana kadar bekle diyoruz. Result eklediğimiz zaman asenkron olmadan çıkıyo senkron oluyo bu sayede bu işlemleri yapıyo sonra diger işlemlere geçiyor. Asenkron oldugu zaman arka tarafta bu işlemleri yaparken çakışıyo çünkü asenkron oldugu zaman başka işlemler de yapılıyor.
                    var result = roleManager.CreateAsync(role).Result;

                }
            }
        }
    }
}
