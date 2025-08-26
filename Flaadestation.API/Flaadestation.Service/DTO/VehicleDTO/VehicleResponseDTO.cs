using Flaadestation.Service.DTO.SharedDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Service.DTO.VehicleDTO
{
    public class VehicleResponseDTO
    {
        public Guid ItemId { get; set; }
        public string Model { get; set; } = string.Empty;
        public string LicensePlate { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public StorageResponseDTO? DefaultStorage { get; set; }
        public Guid CompanyId { get; set; }
        public ImageResponseDTO? Image { get; set; }
        public List<StorageItemResponseDTO> StorageItems { get; set; } = [];
        public List<VehicleEmployeeResponseDTO> Employees { get; set; } = [];
        public List<VehicleToolResponseDTO> Tools { get; set; } = [];
    }

    public class VehicleEmployeeResponseDTO
    {
        public Guid ItemId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public ImageResponseDTO? Image { get; set; }
        public OccupationResponseDTO? Occupation { get; set; }
    }

    public class VehicleToolResponseDTO
    {
        public Guid ItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public ImageResponseDTO? Image { get; set; }
    }
}
