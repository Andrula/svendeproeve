namespace Flaadestation.Service.DTO.CompanyDTO
{
    public class CreateCompanyRequestDTO
    {
        public string Name { get; set; } = string.Empty;
        public Guid AddressId { get; set; }
    }
}
