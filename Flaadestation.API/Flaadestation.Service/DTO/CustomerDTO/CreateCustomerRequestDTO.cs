using Flaadestation.Repository.Database.Entities;

namespace Flaadestation.Service.DTO.CustomerDTO
{
    public class CreateCustomerRequestDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public Guid CompanyId { get; set; }
        public Guid AddressId { get; set; }
    }
}
