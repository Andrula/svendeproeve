using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Service.DTO.CompanyDTO
{
    public class CompanyResponseDTO
    {
        public Guid CompanyId { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
