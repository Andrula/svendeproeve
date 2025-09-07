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
                .Include(e => e.Image)
                .Include(t => t.StorageItems)
                    .ThenInclude(si => si.Storage)
                        .ThenInclude(s => s.Base)
                .Include(t => t.StorageItems)
                    .ThenInclude(si => si.Storage)
                        .ThenInclude(s => s.Job)
                .Include(t => t.DefaultStorage)
                    .ThenInclude(ds => ds.Base)     
                .Include(t => t.DefaultStorage)
                    .ThenInclude(ds => ds.Job)      
                .Where(m => m.CompanyId == companyId)
                .ToListAsync();
        }

        public override async Task<Machinery?> GetByIdAsync(Guid id)
        {
            return await _dbSet
                .Include(t => t.StorageItems)
                    .ThenInclude(si => si.Storage)
                        .ThenInclude(s => s.Base)
                .Include(t => t.StorageItems)
                    .ThenInclude(si => si.Storage)
                        .ThenInclude(s => s.Job)
                .Include(t => t.DefaultStorage)
                .FirstOrDefaultAsync(m => m.ItemId == id);
        }
    }
}
