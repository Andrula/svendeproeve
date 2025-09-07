using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Flaadestation.Shared.Constants;

namespace Flaadestation.Service.DTO.ToolDTO
{
    public class ToolRequestDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public Guid? VehicleId { get; set; }
        public Guid? DefaultStorageId { get; set; }
        public Guid CompanyId { get; set; }
    }
}
