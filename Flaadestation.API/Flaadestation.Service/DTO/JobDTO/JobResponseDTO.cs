using Flaadestation.Service.DTO.SharedDTO;

namespace Flaadestation.Service.DTO.JobDTO
{
    public class JobResponseDTO
    {
        public Guid JobId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime ScheduledStart { get; set; }
        public DateTime ScheduledEnd { get; set; }
        public List<JobCustomerResponseDTO> Customers { get; set; } = [];
        public required JobStorageResponseDTO Storage { get; set; }
    }

    public class JobCustomerResponseDTO
    {
        public Guid CustomerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public Guid CompanyId { get; set; }
        public Guid AddressId { get; set; }
    }

    public class JobStorageResponseDTO
    {
        public Guid StorageId { get; set; }
        public List<StorageItemEmployeeResponseDTO> Employees { get; set; } = [];
        public List<StorageItemVehicleResponseDTO> Vehicles { get; set; } = [];
        public List<StorageItemToolResponseDTO> Tools { get; set; } = [];
        public List<StorageItemMachineryResponseDTO> Machines { get; set; } = [];
        
    }
}
