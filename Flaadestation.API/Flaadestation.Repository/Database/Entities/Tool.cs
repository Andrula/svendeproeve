using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Repository.Database.Entities
{
    public class Tool : Item
    {
        public string Name { get; set; } = string.Empty;
        public Guid? VehicleId { get; set; }

        public Vehicle? Vehicle { get; set; }
    }
}
