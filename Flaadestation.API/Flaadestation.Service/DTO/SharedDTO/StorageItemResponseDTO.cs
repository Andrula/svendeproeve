using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Service.DTO.SharedDTO
{
    public class StorageItemResponseDTO
    {
        public Guid StorageItemId { get; set; }
        public DateTime ScheduledStart { get; set; }
        public DateTime ScheduledEnd { get; set; }
        public string Note { get; set; } = string.Empty;
        public Guid ItemId { get; set; }
        public StorageResponseDTO? Storage { get; set; }
    }
}
