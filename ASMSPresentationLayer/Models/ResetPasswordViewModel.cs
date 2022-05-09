using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASMSPresentationLayer.Models
{
    public class ResetPasswordViewModel
    {
        public string UserId { get; set; }
        public string Code { get; set; }

        [Required(ErrorMessage = "Yeni Şifre Alanı Zorunludur!")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Yeni Şifreniz minumum 8 maksimim 20 haneli olmalıdır!")]
        [Display(Name = "Yeni Şifre")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }


        [Required(ErrorMessage = "Yeni Şifre Tekrar Alanı Zorunludur!")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Yeni Şifreniz minumum 8 maksimim 20 haneli olmalıdır!")]
        [Display(Name = "Yeni Şifre")]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword),ErrorMessage ="Şifreler uyuşmuyor!")]//Bu alan bizim hangi alanla uyuşması gerektiğini söylüyor. Yani yukardaki newpassword alanıyla aynı mı diye kontrol ediyor
        public string ConfirmNewPassword { get; set; }
    }
}
