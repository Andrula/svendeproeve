using Flaadestation.Repository.Database.Entities;
using Flaadestation.Service.DTO.SharedDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Flaadestation.Shared.Constants;

namespace Flaadestation.Service.DTO.ToolDTO
{
    public class ToolResponseDTO
    {
        public Guid ItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public StorageResponseDTO? DefaultStorage { get; set; }
        public Guid CompanyId { get; set; }
        public Guid ImageId { get; set; }
        public byte[]? ImageValue { get; set; }
        public List<ToolStorageItemResponseDTO> StorageItems { get; set; } = [];
        public ToolVehicleResponseDTO? Vehicle { get; set; }
    }

    public class ToolStorageItemResponseDTO
    {
        public Guid StorageItemId { get; set; }
        public DateTime ScheduledStart { get; set; }
        public DateTime ScheduledEnd { get; set; }
        public string Note { get; set; } = string.Empty;
        public StorageResponseDTO? Storage { get; set; }
    }

    public class ToolVehicleResponseDTO
    {
        public Guid VehicleId { get; set; }
        public string Model { get; set; } = string.Empty;
        public string LicensePlate { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public Guid ImageId { get; set; }
        public byte[]? ImageValue { get; set; }
        public StorageResponseDTO? Storage { get; set; }
    }

}
