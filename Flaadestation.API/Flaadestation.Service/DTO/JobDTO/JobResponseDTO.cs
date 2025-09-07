using Flaadestation.Service.DTO.SharedDTO;

namespace Flaadestation.Service.DTO.JobDTO
{
    public class JobResponseDTO
    {
        public Guid JobId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid AddressId { get; set; }
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
        public List<JobStorageDefaultEmployeeResponseDTO> DefaultEmployees { get; set; } = [];
        public List<StorageItemVehicleResponseDTO> Vehicles { get; set; } = [];
        public List<JobStorageDefaultVehicleResponseDTO> DefaultVehicles { get; set; } = [];
        public List<StorageItemToolResponseDTO> Tools { get; set; } = [];
        public List<JobStorageDefaultToolResponseDTO> DefaultTools { get; set; } = [];
        public List<StorageItemMachineryResponseDTO> Machines { get; set; } = [];
        public List<JobStorageDefaultMachineryResponseDTO> DefaultMachines { get; set; } = [];

    }

    public class JobStorageDefaultEmployeeResponseDTO
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

    public class JobStorageDefaultToolResponseDTO
    {
        public Guid ItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;

        public ImageResponseDTO? Image { get; set; }
    }

    public class JobStorageDefaultVehicleResponseDTO
    {
        public Guid ItemId { get; set; }
        public string Model { get; set; } = string.Empty;
        public string LicensePlate { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public ImageResponseDTO? Image { get; set; }

        public List<JobStorageDefaultEmployeeResponseDTO> Employees { get; set; } = [];
        public List<JobStorageDefaultToolResponseDTO> Tools { get; set; } = [];
    }

    public class JobStorageDefaultMachineryResponseDTO
    {
        public Guid ItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;

        public ImageResponseDTO? Image { get; set; }
    }
}
