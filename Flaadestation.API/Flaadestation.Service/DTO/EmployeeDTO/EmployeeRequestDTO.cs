using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Service.DTO.EmployeeDTO
{
    public class EmployeeRequestDTO
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public Guid? OccupationId { get; set; } 
        public Guid? VehicleId { get; set; }
        public Guid DefaultStorageId { get; set; }
        public Guid CompanyId { get; set; }
    }
}
