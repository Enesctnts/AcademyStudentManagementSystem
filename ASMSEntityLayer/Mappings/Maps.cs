using ASMSEntityLayer.Models;
using ASMSEntityLayer.ViewModels;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASMSEntityLayer.Mappings
{
    public class Maps : Profile
    {
        //Buraya Maps ctor metodu gelecektir.
        //içine CreateMap metodu gelecektir

        public Maps()
        {

            //UserAddress'ı UsersAddressesVm'ye dönüştür
            //CreateMap<UsersAddress, UsersAddressVM>();   //DAL--> BLL

            //UsersAddressVm'ı UserAddress'ye dönüştür
            //CreateMap<UsersAddressVM, UsersAddress>();   //PL-->BLL-->DAL

            //Yukardakilerinin aynısını tek seferde yapmak
            //UserAddress ve VM'yi birbirine dönüştür
            CreateMap<UsersAddress, UsersAddressVM>().ReverseMap();
        }
    }
}
