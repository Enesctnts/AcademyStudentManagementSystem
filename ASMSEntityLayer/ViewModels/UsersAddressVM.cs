using ASMSEntityLayer.IdentityModels;
using ASMSEntityLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASMSEntityLayer.ViewModels
{
    public class UsersAddressVM
    {
        public string UserId { get; set; }
        public int Id { get; set; }
        [Required(ErrorMessage = "Mahalle seçimi gereklidir.")]

        public int NeighbourhoodId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; }

        [Required(ErrorMessage ="Adres Başlığı Gereklidir!")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Adres başlığı en az 2 en çok 50 karakter aralığında olmalıdır!!")]
        [Display(Name = "Adres Başlığı")]
        public string AddressTitle { get; set; }

        [StringLength(500, ErrorMessage = "Adres detayı en çok 500 karakter aralığında olabilir!!")]
        [Display(Name = "Adres Detayı")]
        public string AddressDetails { get; set; }

        [StringLength(5, MinimumLength = 5, ErrorMessage = "Posta Kodu 5 karakter olmalıdır!!")]
        [Display(Name = "Posta Kodu")]
        public string PostCode { get; set; } 
        public  AppUser AppUser { get; set; } //ViewModel oluşturulken virtual kullanılmaz normalde public virtual AppUser AppUser şeklindeydi ama viewmodelde kullanmıyoruz.

        public  NeighbourhoodVM Neighbourhood { get; set; }

        //ToDo: ??? Aşagıdakilerle il ve ilçeye ulaşabilir miyim?
        public CityVM City { get; set; }
        public DistrictVM District { get; set; }
    }
}
