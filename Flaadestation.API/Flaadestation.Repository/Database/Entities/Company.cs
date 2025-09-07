using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Repository.Database.Entities
{
    public class Company
    {
        public Guid CompanyId { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid AddressId { get; set; }

        public List<ApplicationUser> Users { get; set; } = [];
        public List<Base> Bases { get; set; } = [];
        public List<Job> Jobs { get; set; } = [];
        public List<Customer> Customers { get; set; } = [];
        public List<License> Licenses { get; set; } = [];

    }
}
