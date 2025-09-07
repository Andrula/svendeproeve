using Flaadestation.Repository.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Repository.Repositories.Interfaces
{
    public interface ILicenseRepository : IRepository<License>
    {
        Task<IEnumerable<License>> AddRangeAsync(License[] licenses);
        Task<License?> GetLicenseByLicenseKeyAsync(Guid licenseKey);
        Task<IEnumerable<License>> GetLicensesByCompanyIdAsync(Guid companyId);
        Task<License?> GetLicensesByUserIdAsync(string userId);
    }
}
