namespace Flaadestation.ASP.DTO.BaseDTO
{
    public class CreateBaseRequestDTO
    {
        public string Name { get; set; } = string.Empty;
        public Guid CompanyId { get; set; }
        public Guid AddressId { get; set; }
    }
}
