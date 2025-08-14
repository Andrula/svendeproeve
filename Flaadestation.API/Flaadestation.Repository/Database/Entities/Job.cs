using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Repository.Database.Entities
{
    public class Job
    {
        public Guid JobId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime ScheduledStart { get; set; }
        public DateTime ScheduledEnd { get; set; }
        public Guid CompanyId { get; set; }
        public Guid StorageId { get; set; }
        public Guid AddressId { get; set; }

        public Company? Company { get; set; }
        public Storage? Storage { get; set; }
        public List<Customer> Customers { get; set; } = [];
    }
}
