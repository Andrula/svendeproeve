using Flaadestation.Repository.Database;
using Flaadestation.Repository.Database.Entities;
using Flaadestation.Repository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Repository.Repositories
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(ApplicationDBContext context) : base(context) { }

        public async Task<IEnumerable<Customer>> GetCustomersByCompanyAsync(Guid companyId)
        {
            return await _dbSet
                .Where(c => c.CompanyId == companyId)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<bool> CustomerExistsByEmailAsync(string email, Guid companyId)
        {
            return await _dbSet
                .AnyAsync(c => c.Email.ToLower() == email.ToLower() && c.CompanyId == companyId);
        }

        public async Task<Customer?> GetCustomerWithJobsAsync(Guid customerId)
        {
            return await _dbSet
                .Include(c => c.Jobs)
                .Include(c => c.Company)
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);
        }

        public override async Task<Customer?> GetByIdAsync(Guid id)
        {
            return await _dbSet
                .Include(c => c.Company)
                .FirstOrDefaultAsync(c => c.CustomerId == id);
        }
    }
}
