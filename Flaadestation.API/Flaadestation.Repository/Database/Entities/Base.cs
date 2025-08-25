using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Repository.Database.Entities
{
    public class Base
    {
        public Guid BaseId { get; set; }
        public string Name {  get; set; } = string.Empty;
        public Guid CompanyId { get; set; }
        public Guid StorageId { get; set; }
        public Guid AddressId { get; set; }

        public Company? Company { get; set; }
        public Storage? Storage { get; set; }
        public List<Employee> Employees { get; set; } = [];
    }
}
