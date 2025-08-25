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
        Task<IEnumerable<BaseResponseDTO>> GetBasesByCompanyAsync(Guid companyId);
        Task<BaseResponseDTO> GetBaseByIdAsync(Guid id);
        Task<Base?> GetBaseWithStorageAsync(Guid baseId);
        Task<BaseResponseDTO> CreateBaseAsync(BaseRequestDTO baseEntity);
        Task<Base?> UpdateBaseAsync(Guid id, BaseRequestDTO baseRequest);
        Task<bool> DeleteBaseAsync(Guid id);
        Task<bool> IsBaseNameAvailableAsync(string name, Guid companyId);
    }
}
