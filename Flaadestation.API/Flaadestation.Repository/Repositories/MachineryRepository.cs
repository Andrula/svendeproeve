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
    public class MachineryRepository : Repository<Machinery>, IMachineryRepository
    {
        public MachineryRepository(ApplicationDBContext dBContext) : base(dBContext) { }
        public async Task<IEnumerable<Machinery>> GetMachineryByCompanyIdAsync(Guid companyId)
        {
            return await _context.Set<Machinery>()
                .Include(m => m.StorageItems)
                    .ThenInclude(si => si.Storage)
                .Where(m => m.CompanyId == companyId)
                .ToListAsync();
        }
        public override async Task<Machinery?> GetByIdAsync(Guid id)
        {
            return await _dbSet
                .Include(m => m.StorageItems)
                    .ThenInclude(si => si.Storage)
                .FirstOrDefaultAsync(m => m.ItemId == id);
        }

    }
}
