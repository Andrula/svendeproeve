using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Service.DTO.LicenseDTO
{
    public class LicenseResponseDTO
    {
        public Guid LicenseId { get; set; }
        public Guid LicenseKey { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public LicenseCompanyResponseDTO? Company { get; set; }
        public LicenseUserResponseDTO? User { get; set; }
    }

    public class LicenseCompanyResponseDTO
    {
        public Guid CompanyId { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class LicenseUserResponseDTO
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsCompanyOwner { get; set; }
    }
}
