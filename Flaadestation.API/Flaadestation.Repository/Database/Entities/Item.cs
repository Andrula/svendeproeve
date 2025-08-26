using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Flaadestation.Shared.Constants;

namespace Flaadestation.Repository.Database.Entities
{
    public class Item
    {
        public Guid ItemId { get; set; }
        public ItemType ItemType { get; set; }
        public string Note { get; set; } = string.Empty;
        public Guid DefaultStorageId { get; set; }
        public Guid CompanyId { get; set; }
        public Guid? ImageId { get; set; }

        public Company? Company { get; set; }
        public Image? Image { get; set; }
        public Storage? DefaultStorage { get; set; }
        public List<StorageItem> StorageItems { get; set; } = [];
        public bool IsAvailableAt(DateTime checkTime)
        {
            var currentAssignment = StorageItems
                .Where(si => si.ScheduledStart <= checkTime && si.ScheduledEnd >= checkTime)
                .FirstOrDefault();

            if (currentAssignment == null)
                return false; 

        
            return currentAssignment.Storage?.Base != null &&
                   currentAssignment.Storage?.Job == null;
        }
    }
}
