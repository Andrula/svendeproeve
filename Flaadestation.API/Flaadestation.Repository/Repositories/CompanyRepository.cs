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
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        public CompanyRepository(ApplicationDBContext dbContext) : base(dbContext) 
        {
            
        }
        public async Task<Company?> GetCompanyWithBasesAsync(Guid companyId)
        {
            return await _dbSet
                .Include(c => c.Bases)
                .FirstOrDefaultAsync(c => c.CompanyId == companyId);
        }

        public async Task<bool> CompanyExistsByNameAsync(string name)
        {
            return await _dbSet
                .AnyAsync(c => c.Name.ToLower() == name.ToLower());
        }
    }
}
