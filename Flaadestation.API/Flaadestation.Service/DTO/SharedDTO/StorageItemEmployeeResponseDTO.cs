using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Service.DTO.SharedDTO
{
    public class StorageItemEmployeeResponseDTO
    {
        public Guid ItemId { get; set; }
        public Guid StorageItemId { get; set; }
        public DateTime ScheduledStart { get; set; }
        public DateTime ScheduledEnd { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public OccupationResponseDTO? Occupation { get; set; }
        public string StorageItemNote { get; set; } = string.Empty;
        public string ItemNote { get; set; } = string.Empty;
        public ImageResponseDTO? Image { get; set; }
    }
}
