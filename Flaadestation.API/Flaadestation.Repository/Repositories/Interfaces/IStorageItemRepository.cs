using Flaadestation.Repository.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Repository.Repositories.Interfaces
{
    public interface IStorageItemRepository : IRepository<StorageItem>
    {
        Task<IEnumerable<StorageItem>> GetStorageItemsByItemIdAsync(Guid itemId);
        Task<StorageItem?> GetActiveStorageItemAsync(Guid itemId, DateTime checkTime);
    }
}
