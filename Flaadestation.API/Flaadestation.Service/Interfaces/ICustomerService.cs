using Flaadestation.Repository.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Service.Interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetCustomersByCompanyAsync(Guid companyId);
        Task<Customer?> GetCustomerByIdAsync(Guid id);
        Task<Customer?> GetCustomerWithJobsAsync(Guid customerId);
        Task<Customer> CreateCustomerAsync(Customer customer);
        Task<Customer?> UpdateCustomerAsync(Guid id, Customer customer);
        Task<bool> DeleteCustomerAsync(Guid id);
        Task<bool> IsCustomerEmailAvailableAsync(string email, Guid companyId);
    }
}
