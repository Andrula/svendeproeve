namespace Flaadestation.ASP.DTO.BaseDTO
{
    public class BaseResponseDTO
    {
        public Guid BaseId { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid CompanyId { get; set; }
        public Guid AddressId { get; set; }
    }
}
