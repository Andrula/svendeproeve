using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Repository.Database.Entities
{
    public class Storage
    {
        public Guid StorageId { get; set; }

        public List<StorageItem> StorageItems { get; set; } = [];

        public Job? Job { get; set; }
        public Base? Base { get; set; }
    }
}
