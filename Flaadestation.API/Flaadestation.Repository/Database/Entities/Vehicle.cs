using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Repository.Database.Entities
{
    public class Vehicle : Item
    {
        public string Model { get; set; } = string.Empty;
        public string LicensePlate { get; set; } = string.Empty;

        public List<Employee> Employees { get; set; } = [];
        public List<Tool> Tools { get; set; } = [];
    }
}
