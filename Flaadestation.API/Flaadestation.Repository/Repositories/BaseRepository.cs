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
    public class BaseRepository : Repository<Base>, IBaseRepository
    {
        public BaseRepository(ApplicationDBContext dBContext) : base(dBContext) { }

        public async Task<IEnumerable<Base>> GetBasesByCompanyAsync(Guid companyId)
        {
            return await _context.Bases
               .Include(j => j.Storage)
                   .ThenInclude(s => s.StorageItems)
                       .ThenInclude(si => si.Item)
               .Include(j => j.Storage)
                   .ThenInclude(s => s.StorageItems)
                       .ThenInclude(si => si.Item)
                           .ThenInclude(i => ((Vehicle)i).Employees)
               .Include(j => j.Storage)
                   .ThenInclude(s => s.StorageItems)
                       .ThenInclude(si => si.Item)
                           .ThenInclude(i => ((Vehicle)i).Tools)
                .Include(j => j.Storage)
                    .ThenInclude(s => s.ItemsWithThisStorageAsDefault)
                        .ThenInclude(i => i.StorageItems)
               .Where(j => j.CompanyId == companyId)
               .ToListAsync();
        }

        public async Task<bool> BaseExistsByNameAsync(string name, Guid companyId)
        {
            return await _dbSet
                .AnyAsync(b => b.Name.ToLower() == name.ToLower() && b.CompanyId == companyId);
        }

        public async Task<Base?> GetBaseWithStorageAsync(Guid baseId)
        {
            return await _dbSet
                .Include(j => j.Storage)
                   .ThenInclude(s => s.StorageItems)
                       .ThenInclude(si => si.Item)
               .Include(j => j.Storage)
                   .ThenInclude(s => s.StorageItems)
                       .ThenInclude(si => si.Item)
                           .ThenInclude(i => ((Vehicle)i).Employees)
               .Include(j => j.Storage)
                   .ThenInclude(s => s.StorageItems)
                       .ThenInclude(si => si.Item)
                           .ThenInclude(i => ((Vehicle)i).Tools)
                .Include(j => j.Storage)
                    .ThenInclude(s => s.ItemsWithThisStorageAsDefault)
                        .ThenInclude(i => i.StorageItems)
                .FirstOrDefaultAsync(b => b.BaseId == baseId);
        }

        public override async Task<Base?> GetByIdAsync(Guid id)
        {
            return await _dbSet
                .Include(j => j.Storage)
                   .ThenInclude(s => s.StorageItems)
                       .ThenInclude(si => si.Item)
               .Include(j => j.Storage)
                   .ThenInclude(s => s.StorageItems)
                       .ThenInclude(si => si.Item)
                           .ThenInclude(i => ((Vehicle)i).Employees)
               .Include(j => j.Storage)
                   .ThenInclude(s => s.StorageItems)
                       .ThenInclude(si => si.Item)
                           .ThenInclude(i => ((Vehicle)i).Tools)
                .Include(j => j.Storage)
                    .ThenInclude(s => s.ItemsWithThisStorageAsDefault)
                        .ThenInclude(i => i.StorageItems)
                .FirstOrDefaultAsync(b => b.BaseId == id);
        }

        public override async Task<IEnumerable<Base>> GetAllAsync()
        {
            return await _dbSet
                .Include(j => j.Storage)
                   .ThenInclude(s => s.StorageItems)
                       .ThenInclude(si => si.Item)
               .Include(j => j.Storage)
                   .ThenInclude(s => s.StorageItems)
                       .ThenInclude(si => si.Item)
                           .ThenInclude(i => ((Vehicle)i).Employees)
               .Include(j => j.Storage)
                   .ThenInclude(s => s.StorageItems)
                       .ThenInclude(si => si.Item)
                           .ThenInclude(i => ((Vehicle)i).Tools)
                .Include(j => j.Storage)
                    .ThenInclude(s => s.ItemsWithThisStorageAsDefault)
                        .ThenInclude(i => i.StorageItems)
                .ToListAsync();
        }
    }
}
