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
    public class BaseService : IBaseService
    {
        private readonly IBaseRepository _baseRepository;
        private readonly IStorageRepository _storageRepository;
        private readonly ApplicationDBContext _context;

        public BaseService(IBaseRepository baseRepository, IStorageRepository storageRepositor, ApplicationDBContext context)
        {
            _baseRepository = baseRepository;
            _storageRepository = storageRepositor;
            _context = context;
        }

        // Metode til at oprette en base med et tilknyttet storage entitet.
        public async Task<Base> CreateBaseAsync(Base baseEntity)
        {
            var nameExists = await _baseRepository.BaseExistsByNameAsync(baseEntity.Name, baseEntity.CompanyId);
            if (nameExists)
            {
                throw new InvalidOperationException($"Base med navnet '{baseEntity.Name}' eksisterer allerede for denne virksomhed.");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var storage = new Storage
                {
                    StorageId = baseEntity.StorageId,
                    Base = null, 
                    Job = null  
                };

                await _storageRepository.AddAsync(storage);

                var createdBase = await _baseRepository.AddAsync(baseEntity);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return createdBase;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        /// Metode til at slette base.
        public async Task<bool> DeleteBaseAsync(Guid id)
        {
            var baseEntity = await _baseRepository.GetBaseWithStorageAsync(id);
            if (baseEntity == null)
                return false;

            if (baseEntity.Storage?.StorageItems.Any() == true)
            {
                throw new InvalidOperationException("Kan ikke slette base der har ting opbevaret. Flyt alle items først.");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                if (baseEntity.Storage != null)
                {
                     _storageRepository.Delete(baseEntity.Storage.StorageId);
                }

                _baseRepository.Delete(id);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        // Metode til at hente base via. ID.
        public async Task<Base?> GetBaseByIdAsync(Guid id)
        {
            return await _baseRepository.GetByIdAsync(id);
        }

        // Metode til at hente base med en tilknyttet storage.
        public async Task<Base?> GetBaseWithStorageAsync(Guid baseId)
        {
            return await _baseRepository.GetBaseWithStorageAsync(baseId);
        }

        // Metode til at hente virksomhedens baser.
        public async Task<IEnumerable<Base>> GetBasesByCompanyAsync(Guid companyId)
        {
            return await _baseRepository.GetBasesByCompanyAsync(companyId);
        }

        // Metode til at om navn på base allerede eksisterer.
        public async Task<bool> IsBaseNameAvailableAsync(string name, Guid companyId)
        {
            return !await _baseRepository.BaseExistsByNameAsync(name, companyId);
        }

        // Metode til at opdatere informmationer på base.
        public async Task<Base?> UpdateBaseAsync(Guid id, string name)
        {
            var existingBase = await _baseRepository.GetByIdAsync(id);
            if (existingBase == null)
                return null;

            if (existingBase.Name != name)
            {
                var nameExists = await _baseRepository.BaseExistsByNameAsync(name, existingBase.CompanyId);
                if (nameExists)
                {
                    throw new InvalidOperationException($"Base med navnet '{name}' eksisterer allerede for denne virksomhed.");
                }
            }

            existingBase.Name = name;
            _baseRepository.Update(existingBase);
            await _baseRepository.SaveChangesAsync();
            return existingBase;
        }
    }
}
