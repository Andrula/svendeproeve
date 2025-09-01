using Flaadestation.Service.DTO.LicenseDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Service.Interfaces
{
    public interface ILicenseService
    {
        Task<IEnumerable<LicenseResponseDTO>> AddRangeByCompanyIdAsync(Guid companyId, int amount);
        Task<LicenseResponseDTO?> GetLicenseByLicenseKeyAsync(Guid licenseKey);
        Task<IEnumerable<LicenseResponseDTO>> GetLicensesByCompanyIdAsync(Guid companyId);
        Task<LicenseResponseDTO?> GetLicensesByUserIdAsync(string userId);
        Task<LicenseResponseDTO> CreateLicenseAsync(LicenseRequestDTO licenseRequest);
        Task<LicenseResponseDTO?> UpdateLicenseByIdAsync(Guid licenseId, LicenseRequestDTO licenseRequest);
        Task<bool> DeleteLicenseAsync(Guid licenseId);
    }
}
