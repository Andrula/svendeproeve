using Flaadestation.Repository.Database.Entities;
using Flaadestation.Service.DTO.EmployeeDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Service.Interfaces
{
    public interface IEmployeeService
    {
        Task<EmployeeResponseDTO> CreateEmployeeAsync(EmployeeRequestDTO employeeRequest);
        Task<bool> DeleteEmployeeAsync(Guid employeeId);
        Task<EmployeeResponseDTO?> GetEmployeeByIdAsync(Guid employeeId);
        Task<IEnumerable<EmployeeResponseDTO>> GetEmployeesByCompanyAsync(Guid companyId);
        Task<EmployeeResponseDTO?> UpdateEmployeeAsync(Guid employeeId, EmployeeRequestDTO employeeRequest);
    }
}
