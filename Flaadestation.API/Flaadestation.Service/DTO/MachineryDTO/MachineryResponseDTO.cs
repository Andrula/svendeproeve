using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Service.DTO.MachineryDTO
{
    public class MachineryResponseDTO
    {
        public Guid MachineryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public Guid CompanyId { get; set; }
        public Guid ImageId { get; set; }
        public bool IsCurrentlyAvailable { get; set; }
    }
}
