using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Service.DTO.SharedDTO
{
    public class OccupationResponseDTO
    {
        public Guid OccupationId { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
