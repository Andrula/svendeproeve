using System.ComponentModel.DataAnnotations;

namespace Flaadestation.Service.DTO.JobDTO
{
    public class JobRequestDTO
    {
        [Required]
        [StringLength(80, ErrorMessage = "Job title cannot be longer than 80 characters")]
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime ScheduledStart { get; set; }

        public DateTime ScheduledEnd { get; set; }

        [Required]
        public Guid CompanyId { get; set; }

        [Required]
        public Guid AddressId { get; set; }
    }
}
