using Flaadestation.Service.DTO.JobDTO;
using Flaadestation.Service.DTO.SharedDTO;

namespace Flaadestation.Service.DTO.BaseDTO
{
    public class BaseResponseDTO
    {
        public Guid BaseId { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid CompanyId { get; set; }
        public Guid AddressId { get; set; }
        public required BaseStorageResponseDTO Storage {  get; set; }   
    }

    public class BaseStorageResponseDTO
    {
        public Guid StorageId { get; set; }
        public List<StorageItemEmployeeResponseDTO> Employees { get; set; } = [];
        public List<BaseStorageDefaultEmployeeResponseDTO> DefaultEmployees { get; set; } = [];
        public List<StorageItemVehicleResponseDTO> Vehicles { get; set; } = [];
        public List<BaseStorageDefaultVehicleResponseDTO> DefaultVehicles { get; set; } = [];
        public List<StorageItemToolResponseDTO> Tools { get; set; } = [];
        public List<BaseStorageDefaultToolResponseDTO> DefaultTools { get; set; } = [];
        public List<StorageItemMachineryResponseDTO> Machines { get; set; } = [];
        public List<BaseStorageDefaultMachineryResponseDTO> DefaultMachines { get; set; } = [];
    }

    public class BaseStorageDefaultEmployeeResponseDTO
    {
        public Guid ItemId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public OccupationResponseDTO? Occupation { get; set; }

        public ImageResponseDTO? Image { get; set; }
    }

    public class BaseStorageDefaultToolResponseDTO
    {
        public Guid ItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;

        public ImageResponseDTO? Image { get; set; }
    }

    public class BaseStorageDefaultVehicleResponseDTO
    {
        public Guid ItemId { get; set; }
        public string Model { get; set; } = string.Empty;
        public string LicensePlate { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public ImageResponseDTO? Image { get; set; }

        public List<BaseStorageDefaultEmployeeResponseDTO> Employees { get; set; } = [];
        public List<BaseStorageDefaultToolResponseDTO> Tools { get; set; } = [];
    }

    public class BaseStorageDefaultMachineryResponseDTO
    {
        public Guid ItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;

        public ImageResponseDTO? Image { get; set; }
    }
}
