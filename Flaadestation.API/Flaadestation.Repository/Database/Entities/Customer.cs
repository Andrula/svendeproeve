using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Repository.Database.Entities
{
    public class Customer
    {
        public Guid CustomerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public Guid CompanyId { get; set; }
        public Guid AddressId { get; set; }

        public Company? Company { get; set; }
        public List<Job> Jobs { get; set; } = new List<Job>();
    }
}
