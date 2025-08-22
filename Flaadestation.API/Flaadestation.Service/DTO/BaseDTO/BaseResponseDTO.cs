using Flaadestation.Service.DTO.JobDTO;

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
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public Guid CompanyId { get; set; }
    }

    public class BaseStorageResponseDTO
    {
        public Guid StorageId { get; set; }
        public List<BaseStorageItemReponseDTO> StorageItems { get; set; } = [];
    }

    public class BaseStorageItemReponseDTO
    {
        public Guid StorageItemId { get; set; }
        public DateTime ScheduledStart { get; set; }
        public DateTime ScheduledEnd { get; set; }
        public string Note { get; set; } = string.Empty;
    }
}
