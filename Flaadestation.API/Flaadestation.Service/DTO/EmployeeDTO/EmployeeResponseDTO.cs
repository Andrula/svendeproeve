using Flaadestation.Service.DTO.SharedDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Service.DTO.EmployeeDTO
{
    public class EmployeeResponseDTO
    {
        public Guid ItemId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public EmployeeOccupationResponseDTO? Occupation { get; set; }
        public StorageResponseDTO? DefaultStorage { get; set; }
        public Guid CompanyId { get; set; }
        public ImageResponseDTO? Image { get; set; }
        public List<StorageItemResponseDTO> StorageItems { get; set; } = [];
        public EmployeeVehicleResponseDTO? Vehicle { get; set; } 
    }

    public class EmployeeVehicleResponseDTO
    {
        public Guid VehicleId { get; set; }
        public string Model { get; set; } = string.Empty;
        public string LicensePlate { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public ImageResponseDTO? Image { get; set; }
        public StorageResponseDTO? Storage { get; set; }
        public List<StorageItemResponseDTO?> StorageItems { get; set; } = [];
    }

    public class EmployeeOccupationResponseDTO
    {
        public Guid OccupationId { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
