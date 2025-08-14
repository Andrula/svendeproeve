using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Repository.Database.Entities
{
    public class StorageItem
    {
        public Guid StorageItemId { get; set; }
        public DateTime ScheduledStart { get; set; }
        public DateTime ScheduledEnd { get; set; }
        public string Note { get; set; } = string.Empty;
        public Guid ItemId { get; set; }
        public Guid StorageId { get; set; }

        public Item? Item { get; set; }
        public Storage? Storage { get; set; }
    }
}
