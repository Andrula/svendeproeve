using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Repository.Database.Entities
{
    public class Occupation
    {
        public Guid OccupationId { get; set; }
        public string Name { get; set; } = string.Empty;

        public List<Employee> Employees { get; set; } = [];
    }
}
