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
    public class StorageItemRepository : Repository<StorageItem>, IStorageItemRepository
    {
        public StorageItemRepository(ApplicationDBContext dBContext) : base(dBContext) { }

        public async Task<IEnumerable<StorageItem>> GetStorageItemsByItemIdAsync(Guid itemId)
        {
            return await _context.StorageItems
                .Include(si => si.Storage)
                .Where(si => si.ItemId == itemId).ToListAsync();
        }

        public async Task<IEnumerable<StorageItem>> GetStorageItemsByStorageIdAsync(Guid storageId)
        {
            return await _context.StorageItems
                .Include(si => si.Storage)
                .Where(si => si.StorageId == storageId).ToListAsync();
        }
    }
}
