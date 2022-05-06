using ASMSEntityLayer.IdentityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASMSEntityLayer.ViewModels
{
    public class StudentVM
    {
        
        [StringLength(11, MinimumLength = 11, ErrorMessage = "TCKimlik numarası 11 haneli olmalıdır!")]
        public string TCNumber { get; set; }
        public string UserId { get; set; }
        public virtual AppUser AppUser { get; set; }

        //bu VM genişeletilebilir. Örneğin öğrencinin aldıgı kurslar, öğrencinin devamsızlık durumu (yoklama bilgiisi)

    }
}
