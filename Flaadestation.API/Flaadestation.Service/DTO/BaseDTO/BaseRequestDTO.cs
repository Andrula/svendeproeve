namespace Flaadestation.Service.DTO.BaseDTO
{
    public class BaseRequestDTO
    {
        public string Name { get; set; } = string.Empty;
        public Guid CompanyId { get; set; }
        public Guid AddressId { get; set; }
    }
}
