using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Repository.Database.Entities
{
    public class Employee : Item
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public Guid? OccupationId { get; set; }
        public Guid? VehicleId { get; set; } 

        public Occupation? Occupation { get; set; }
        public Vehicle? Vehicle { get; set; }

    }
}
