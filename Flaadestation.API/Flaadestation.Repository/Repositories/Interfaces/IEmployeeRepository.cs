using Flaadestation.Repository.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Repository.Repositories.Interfaces
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<IEnumerable<Employee>> GetEmployeesByOccupationAsync(Guid occupationId);
        Task<IEnumerable<Employee>> GetAvailableEmployeesAsync(DateTime? startTime, DateTime? endTime);
        Task<IEnumerable<Employee>> GetEmployeesByCompanyIdAsync(Guid companyId);
    }
}
