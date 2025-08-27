using Flaadestation.Repository.Database.Entities;
using Flaadestation.Repository.Repositories.Interfaces;
using Flaadestation.Service.DTO.MachineryDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Service.Interfaces
{
    public interface IMachineryService
    {
        Task<IEnumerable<MachineryResponseDTO>> GetAllAsync();
        Task<IEnumerable<MachineryResponseDTO>> GetMachineryByCompanyIdAsync(Guid companyId);
        Task<IEnumerable<MachineryResponseDTO>> GetAvailableMachineryAsync(Guid companyId, DateTime? startDate = null, DateTime? endDate = null);
        Task<MachineryResponseDTO?> GetMachineryByIdAsync(Guid id);
        Task<MachineryResponseDTO> CreateMachineryAsync(MachineryRequestDTO request);
        Task<bool> DeleteMachineryAsync(Guid id);
        Task<MachineryResponseDTO?> UpdateMachineryAsync(Guid id, MachineryRequestDTO request);
        Task<bool> ExistsAsync(Guid id);
        Task<Machinery?> GetByIdAsync(Guid id);
    }
}
