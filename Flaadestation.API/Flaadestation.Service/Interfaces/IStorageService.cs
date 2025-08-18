using Flaadestation.Repository.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Service.Interfaces
{
    public interface IStorageService
    {
        Task<Storage?> GetStorageByIdAsync(Guid id);
        Task<Storage?> GetStorageWithItemsAsync(Guid storageId);
        Task<IEnumerable<Storage>> GetJobStoragesAsync();
        Task<IEnumerable<Storage>> GetBaseStoragesAsync();
    }
}
