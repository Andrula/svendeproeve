using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Service.DTO.LicenseDTO
{
    public class LicenseRequestDTO
    {
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

        [Required]
        public Guid CompanyId { get; set; }
        [Required]
        public string UserId { get; set; } = string.Empty;
    }
}
