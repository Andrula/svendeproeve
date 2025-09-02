using Flaadestation.Repository.Database;
using Flaadestation.Repository.Database.Entities;
using Flaadestation.Repository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Repository.Repositories
{
    public class LicenseRepository : Repository<License>, ILicenseRepository
    {
        public LicenseRepository(ApplicationDBContext dbContext) : base(dbContext)
        {

        }

        public async Task<IEnumerable<License>> AddRangeAsync(License[] licenses)
        {

            await _context.Licenses.AddRangeAsync(licenses);
            await _context.SaveChangesAsync();
            return licenses;
        }

        public async Task<License?> GetLicenseByLicenseKeyAsync(Guid licenseKey)
        {
            return await _context.Licenses
                .Include(l => l.Company)
                .Include(l => l.User)
                .FirstOrDefaultAsync(l => l.LicenseKey == licenseKey);
        }

        public async Task<IEnumerable<License>> GetLicensesByCompanyIdAsync(Guid companyId)
        {
            return await _context.Licenses
                .Include(l => l.Company)
                .Include(l => l.User)
                .Where(l => l.CompanyId == companyId)
                .ToListAsync();
        }

        public async Task<License?> GetLicensesByUserIdAsync(string userId)
        {
            return await _context.Licenses
                .Include(l => l.Company)
                .Include(l => l.User)
                .FirstOrDefaultAsync(l => l.UserId == userId);
        }
    }
}
