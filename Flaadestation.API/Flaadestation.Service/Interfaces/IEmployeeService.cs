using Flaadestation.Repository.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Service.Interfaces
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Employee>> GetAllEmployeesAsync();
        Task<Employee?> GetEmployeeByIdAsync(Guid id);
        Task<Employee> CreateEmployeeAsyncToBase(Employee employee);
        Task<Employee?> UpdateEmployeeAsync(Guid id, Employee employee);
        Task<bool> DeleteEmployeeAsync(Guid id);
    }
}
