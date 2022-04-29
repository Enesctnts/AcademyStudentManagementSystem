using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASMSEntityLayer.ViewModels
{
    public class CityVM
    {
        
        public byte Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "İl adı en az 2 en çok 50 karakter aralığında olmalıdır!!")]
        public string CityName { get; set; }

        [Required]
        // TODO is unique core da nasıl yapılır?
        public byte PlateCode { get; set; }
        //buraya geri döneceği<
        public  ICollection<DistrictVM> Districts { get; set; }
    }
}
