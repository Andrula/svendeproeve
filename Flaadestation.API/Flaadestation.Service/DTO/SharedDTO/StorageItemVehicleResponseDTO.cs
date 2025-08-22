using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Service.DTO.SharedDTO
{
    public class StorageItemVehicleResponseDTO
    {
        public Guid ItemId { get; set; }
        public Guid StorageItemId { get; set; }
        public DateTime ScheduledStart { get; set; }
        public DateTime ScheduledEnd { get; set; }
        public string Model { get; set; } = string.Empty;
        public string LicensePlate { get; set; } = string.Empty;
        public string StorageItemNote { get; set; } = string.Empty;
        public string ItemNote { get; set; } = string.Empty;
        public Guid ImageId { get; set; }
        public byte[]? ImageValue { get; set; }

        public List<StorageItemVehicleEmployeeResponseDTO> Employees { get; set; } = [];
        public List<StorageItemVehicleToolResponseDTO> Tools { get; set; } = [];
    }

    public class StorageItemVehicleEmployeeResponseDTO
    {
        public Guid ItemId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public Guid? OccupationId { get; set; }
        public string Occupation { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public Guid ImageId { get; set; }
        public byte[]? ImageValue { get; set; }
    }

    public class StorageItemVehicleToolResponseDTO
    {
        public Guid ItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public Guid ImageId { get; set; }
        public byte[]? ImageValue { get; set; }
    }
}
