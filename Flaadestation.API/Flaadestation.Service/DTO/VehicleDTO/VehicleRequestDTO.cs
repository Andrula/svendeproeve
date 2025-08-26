using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Service.DTO.VehicleDTO
{
    public class VehicleRequestDTO
    {
        public required string Model { get; set; }
        public required string LicensePlate { get; set; }
        public required string Note { get; set; }
        public Guid DefaultStorageId { get; set; }
        public byte[]? ImageValue { get; set; }
    } 
}
