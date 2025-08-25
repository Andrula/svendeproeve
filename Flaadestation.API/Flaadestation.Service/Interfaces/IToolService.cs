using Flaadestation.Service.DTO.ToolDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Service.Interfaces
{
    public interface IToolService
    {
        Task<IEnumerable<ToolResponseDTO>> GetToolsByCompanyAsync(Guid companyId);
        Task<ToolResponseDTO?> GetToolByIdAsync(Guid toolId);
        Task<ToolResponseDTO> CreateToolAsync(ToolRequestDTO toolRequest);
        Task<ToolResponseDTO?> UpdateToolAsync(Guid toolId, ToolRequestDTO toolRequest);
        Task<bool> DeleteToolAsync(Guid toolId);
    }
}
