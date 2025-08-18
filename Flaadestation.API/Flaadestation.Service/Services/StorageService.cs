using Flaadestation.Repository.Database;
using Flaadestation.Repository.Database.Entities;
using Flaadestation.Repository.Repositories.Interfaces;
using Flaadestation.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Service.Services
{
    public class StorageService : IStorageService
    {
        private readonly IStorageRepository _storageRepository;
        public StorageService(IStorageRepository storageRepository)
        {
            _storageRepository = storageRepository;
        }

        // Metode til og hente 
        public async Task<IEnumerable<Storage>> GetBaseStoragesAsync()
        {
            return await _storageRepository.GetBaseStoragesAsync();
        }

        public async Task<IEnumerable<Storage>> GetJobStoragesAsync()
        {
            return await _storageRepository.GetJobStoragesAsync();
        }

        public async Task<Storage?> GetStorageByIdAsync(Guid id)
        {
            return await _storageRepository.GetByIdAsync(id);
        }

        public async Task<Storage?> GetStorageWithItemsAsync(Guid storageId)
        {
            return await _storageRepository.GetStorageWithItemsAsync(storageId);
        }
    }
}
