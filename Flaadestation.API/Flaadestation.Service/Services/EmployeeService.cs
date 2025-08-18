using Flaadestation.Repository.Database.Entities;
using Flaadestation.Repository.Repositories.Interfaces;
using Flaadestation.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Service.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<Employee> CreateEmployeeAsyncToBase(Employee employee)
        {
            var newEmployee = await _employeeRepository.AddAsync(employee);
            await _employeeRepository.SaveChangesAsync();
            return newEmployee;
        }

        public Task<bool> DeleteEmployeeAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Employee?> GetEmployeeByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Employee?> UpdateEmployeeAsync(Guid id, Employee employee)
        {
            throw new NotImplementedException();
        }
    }
}
