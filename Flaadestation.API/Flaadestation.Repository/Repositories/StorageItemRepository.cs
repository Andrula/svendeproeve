using Flaadestation.Repository.Database;
using Flaadestation.Repository.Database.Entities;
using Flaadestation.Repository.Repositories.Interfaces;
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

        public Task<StorageItem?> GetActiveStorageItemAsync(Guid itemId, DateTime checkTime)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<StorageItem>> GetStorageItemsByItemIdAsync(Guid itemId)
        {
            throw new NotImplementedException();
        }
    }
}
