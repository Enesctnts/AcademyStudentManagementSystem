using ASMSEntityLayer.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASMSPresentationLayer.Models
{
    public class RegisterViewModel
    {

        [Required]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "TC 11 haneli olmalıdır!")]
        public string TCNumber { get; set; }


        [Required(ErrorMessage = "İsim Gereklidir!")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "İsminiz en az 2 en çok 50 karakter olmalıdır!")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Soyisim Gereklidir!")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Soyisminiz en az 2 en çok 50 karakter olmalıdır!")]
        public string Surname { get; set; }


        [Required(ErrorMessage ="Email Zorunludur!")]
        [EmailAddress]
        public string Email { get; set; }


        [Required(ErrorMessage ="Şifre Alanı Zorunludur!")]
        [StringLength(20,MinimumLength =8,ErrorMessage ="Şifreniz minumum 8 maksimim 20 haneli olmalıdır!")]
        [Display(Name="Şifre")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }


        [Required(ErrorMessage ="Cinsiyet Seçimi Gereklidir1")]
        public Genders Gender { get; set; }

    }
}
