using Flaadestation.Repository.Database.Entities;
using Flaadestation.Repository.Repositories.Interfaces;
using Flaadestation.Service.DTO.MachineryDTO;
using Flaadestation.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public Task<MachineryResponseDTO> CreateMachineryAsync(MachineryRequestDTO request)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Machinery>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MachineryResponseDTO>> GetAvailableMachineryAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            throw new NotImplementedException();
        }

        public Task<Machinery?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MachineryResponseDTO>> GetMachineryByCompanyAsync(Guid companyId)
        {
            throw new NotImplementedException();
        }

        public async Task<MachineryResponseDTO?> GetMachineryByIdAsync(Guid id)
        {
            var machinery = await _machineryRepository.GetByIdAsync(id);
            return machinery is null ? null : MapMachineryToResponse(machinery);
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
                ImageId = request.ImageId ?? Guid.Empty
            };
        }

        private MachineryResponseDTO MapMachineryToResponse(Machinery machinery)
        {
            return new MachineryResponseDTO
            {
                Name = machinery.Name,
                Note = machinery.Note,
                CompanyId = machinery.CompanyId,
                ImageId = machinery.ImageId,
                IsCurrentlyAvailable = IsAvailableDuringPeriod(machinery.StorageItems, DateTime.Now, DateTime.Now)
            };
        }
    }
}
