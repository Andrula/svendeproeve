using Flaadestation.Repository.Database.Entities;
using Flaadestation.Service.DTO.CompanyDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Service.Interfaces
{
    public interface ICompanyService
    {
        Task<CompanyResponseDTO?> GetCompanyByIdAsync(Guid id);
        Task<CompanyResponseDTO> CreateCompanyAsync(CompanyRequestDTO companyRequest);
        Task<CompanyResponseDTO?> UpdateCompanyAsync(Guid id, CompanyRequestDTO companyRequest);
        Task<bool> DeleteCompanyAsync(Guid id);
        Task<bool> IsCompanyNameAvailableAsync(string name);
    }
}
