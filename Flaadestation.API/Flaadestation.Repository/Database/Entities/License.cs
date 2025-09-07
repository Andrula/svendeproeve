using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Repository.Database.Entities
{
    public class License
    {
        public Guid LicenseId { get; set; }
        public Guid LicenseKey { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public Guid CompanyId { get; set; }
        public string? UserId { get; set; }

        public Company? Company { get; set; }
        public ApplicationUser? User { get; set; }
    }
}
