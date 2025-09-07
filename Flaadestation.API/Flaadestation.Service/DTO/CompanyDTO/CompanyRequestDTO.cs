namespace Flaadestation.Service.DTO.CompanyDTO
{
    public class CompanyRequestDTO
    {
        public string Name { get; set; } = string.Empty;
        public Guid AddressId { get; set; }
    }
}
