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
        Task<IEnumerable<MachineryResponseDTO>> GetMachineryByCompanyAsync(Guid companyId);
        Task<IEnumerable<MachineryResponseDTO>> GetAvailableMachineryAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<MachineryResponseDTO?> GetMachineryByIdAsync(Guid id);
        Task<MachineryResponseDTO> CreateMachineryAsync(MachineryRequestDTO request);
    }
}
