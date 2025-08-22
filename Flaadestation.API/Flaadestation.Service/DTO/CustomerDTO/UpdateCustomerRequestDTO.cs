namespace Flaadestation.Service.DTO.CustomerDTO
{
    public class UpdateCustomerRequestDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public Guid AddressId { get; set; }
    }
}
