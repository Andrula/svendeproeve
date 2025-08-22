using Flaadestation.Repository.Database.Entities;
using Flaadestation.Service.DTO.BaseDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Service.Interfaces
{
    public interface IBaseService
    {
        Task<IEnumerable<Base>> GetBasesByCompanyAsync(Guid companyId);
        Task<BaseRequestDTO> GetBaseByIdAsync(Guid id);
        Task<Base?> GetBaseWithStorageAsync(Guid baseId);
        Task<BaseResponseDTO> CreateBaseAsync(BaseRequestDTO baseEntity);
        Task<Base?> UpdateBaseAsync(Guid id, string name);
        Task<bool> DeleteBaseAsync(Guid id);
        Task<bool> IsBaseNameAvailableAsync(string name, Guid companyId);
    }
}
