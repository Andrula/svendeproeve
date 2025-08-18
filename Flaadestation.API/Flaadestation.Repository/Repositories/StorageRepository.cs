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
    public class StorageRepository : Repository<Storage>, IStorageRepository
    {
        public StorageRepository(ApplicationDBContext dBContext) : base(dBContext) { }

        public async Task<Storage?> GetStorageWithItemsAsync(Guid storageId)
        {
            return await _dbSet
                .Include(s => s.StorageItems)
                    .ThenInclude(si => si.Item)
                .Include(s => s.Base)
                .Include(s => s.Job)
                .FirstOrDefaultAsync(s => s.StorageId == storageId);
        }

        public async Task<IEnumerable<Storage>> GetJobStoragesAsync()
        {
            return await _dbSet
                .Where(s => s.Job != null)
                .Include(s => s.Job)
                .Include(s => s.StorageItems)
                .ToListAsync();
        }

        public async Task<IEnumerable<Storage>> GetBaseStoragesAsync()
        {
            return await _dbSet
                .Where(s => s.Base != null)
                .Include(s => s.Base)
                .Include(s => s.StorageItems)
                .ToListAsync();
        }

        public override async Task<Storage?> GetByIdAsync(Guid id)
        {
            return await _dbSet
                .Include(s => s.Base)
                .Include(s => s.Job)
                .FirstOrDefaultAsync(s => s.StorageId == id);
        }
    }
}
