using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Service.DTO.MachineryDTO
{
    public class MachineryRequestDTO
    {
        public string Name { get; set; } = string.Empty;
        public Guid CompanyId { get; set; }
        public string? Note { get; set; }
        public Guid DefaultStorageId { get; set; }
    }
}
