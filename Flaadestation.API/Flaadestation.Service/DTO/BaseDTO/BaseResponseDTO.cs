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
        public List<BaseEmployeeResponseDTO> Employees { get; set; } = [];
        public required BaseStorageResponseDTO Storage {  get; set; }   
    }

    public class BaseEmployeeResponseDTO
    {
        public Guid EmployeeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

        public Guid CompanyId { get; set; }
    }

    public class BaseStorageResponseDTO
    {
        public Guid StorageId { get; set; }
        public List<StorageItemEmployeeResponseDTO> Employees { get; set; } = [];
        public List<StorageItemVehicleResponseDTO> Vehicles { get; set; } = [];
        public List<StorageItemToolResponseDTO> Tools { get; set; } = [];
        public List<StorageItemMachineryResponseDTO> Machines { get; set; } = [];
    }
}
