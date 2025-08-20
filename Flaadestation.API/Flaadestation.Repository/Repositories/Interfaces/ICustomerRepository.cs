using Flaadestation.Repository.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Repository.Repositories.Interfaces
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<IEnumerable<Customer>> GetCustomersByCompanyAsync(Guid companyId);
        Task<bool> CustomerExistsByEmailAsync(string email, Guid companyId);
        Task<Customer?> GetCustomerWithJobsAsync(Guid customerId);
    }
}
