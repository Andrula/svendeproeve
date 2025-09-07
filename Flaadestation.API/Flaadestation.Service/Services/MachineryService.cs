using Flaadestation.Repository.Database.Entities;
using Flaadestation.Repository.Repositories.Interfaces;
using Flaadestation.Service.DTO.MachineryDTO;
using Flaadestation.Service.DTO.SharedDTO;
using Flaadestation.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using static Flaadestation.Shared.Constants;

namespace Flaadestation.Service.Services
{
    public class MachineryService : IMachineryService
    {
        private readonly IMachineryRepository _machineryRepository;
        private readonly IStorageItemRepository _storageItemRepository;
        public MachineryService(IMachineryRepository machineryRepository, IStorageItemRepository storageItemRepository)
        {
            _machineryRepository = machineryRepository;
            _storageItemRepository = storageItemRepository;
        }
        public async Task<MachineryResponseDTO> CreateMachineryAsync(MachineryRequestDTO request)
        {
            var machinery = MapRequestToMachinery(request);
            var created = await _machineryRepository.AddAsync(machinery);
            await _machineryRepository.SaveChangesAsync();
            return MapMachineryToResponse(created);
        }
        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _machineryRepository.ExistsAsync(id);
        }
        public async Task<IEnumerable<MachineryResponseDTO>> GetAllAsync()
        {
            var machinery = await _machineryRepository.GetAllAsync();
            return machinery.Select(MapMachineryToResponse);
        }
        public async Task<IEnumerable<MachineryResponseDTO>> GetAvailableMachineryAsync(Guid companyId, DateTime? startDate = null, DateTime? endDate = null)
        {
            var machines = await _machineryRepository.GetMachineryByCompanyIdAsync(companyId);
            if (startDate.HasValue && endDate.HasValue)
            {
                var available = machines.Where(m =>
                    IsAvailableDuringPeriod(m.StorageItems, startDate.Value, endDate.Value));
                return available.Select(MapMachineryToResponse);
            }
            var now = DateTime.Now;
            var currentlyAvailable = machines.Where(m =>
                IsAvailableDuringPeriod(m.StorageItems, now, now));
            return currentlyAvailable.Select(MapMachineryToResponse);
        }
        public async Task<IEnumerable<MachineryResponseDTO>> GetMachineryByCompanyIdAsync(Guid companyId)
        {
            var machinery = await _machineryRepository.GetMachineryByCompanyIdAsync(companyId);
            return machinery.Select(MapMachineryToResponse);
        }
        public async Task<Machinery?> GetByIdAsync(Guid id)
        {
            return await _machineryRepository.GetByIdAsync(id);
        }
        public async Task<MachineryResponseDTO?> GetMachineryByIdAsync(Guid id)
        {
            var machinery = await _machineryRepository.GetByIdAsync(id);
            return machinery is null ? null : MapMachineryToResponse(machinery);
        }
        public async Task<bool> DeleteMachineryAsync(Guid id)
        {
            var machinery = await _machineryRepository.GetByIdAsync(id);
            if (machinery == null)
                return false;

            if (machinery.StorageItems.Any(si => si.ScheduledEnd >= DateTime.Now))
            {
                throw new InvalidOperationException("Kan ikke slette maskine der er planlagt til brug. Fjern alle planlagte opgaver først.");
            }

            _machineryRepository.Delete(id);
            await _machineryRepository.SaveChangesAsync();
            return true;
        }

        public async Task<MachineryResponseDTO?> UpdateMachineryAsync(Guid id, MachineryRequestDTO request)
        {
            var existing = await _machineryRepository.GetByIdAsync(id);
            if (existing == null)
                return null;

            existing.Name = request.Name;
            existing.Note = request.Note ?? string.Empty;
            existing.DefaultStorageId = request.DefaultStorageId;

            _machineryRepository.Update(existing);
            await _machineryRepository.SaveChangesAsync();
            return MapMachineryToResponse(existing);
        }

        private bool IsAvailableDuringPeriod(List<StorageItem> storageItems, DateTime startDate, DateTime endDate)
        {
            return !storageItems.Any(si =>
                si.ScheduledStart < endDate && si.ScheduledEnd > startDate);
        }
        private Machinery MapRequestToMachinery(MachineryRequestDTO request)
        {
            return new Machinery
            {
                ItemId = Guid.NewGuid(),
                Name = request.Name,
                CompanyId = request.CompanyId,
                ItemType = ItemType.Machine,
                Note = request.Note ?? string.Empty,
                DefaultStorageId = request.DefaultStorageId
            };
        }
        private MachineryResponseDTO MapMachineryToResponse(Machinery machinery)
        {
            return new MachineryResponseDTO
            {
                ItemId = machinery.ItemId,
                Name = machinery.Name,
                Note = machinery.Note,
                CompanyId = machinery.CompanyId,
                ImageId = machinery.ImageId,
                DefaultStorage = machinery.DefaultStorage is null ? null : StorageResponseDTO.MapStorageToStorageResponseDTO(machinery.DefaultStorage),
                StorageItems = machinery.StorageItems.Select(si => new StorageItemResponseDTO
                {
                    StorageItemId = si.StorageItemId,
                    Storage = StorageResponseDTO.MapStorageToStorageResponseDTO(si.Storage!),
                    ScheduledStart = si.ScheduledStart,
                    ScheduledEnd = si.ScheduledEnd,
                    Note = si.Note,
                    ItemId = si.ItemId,
                }).ToList(),
            };
        }
    }
}