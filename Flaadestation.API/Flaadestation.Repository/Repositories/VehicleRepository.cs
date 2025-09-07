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
    public class VehicleRepository : Repository<Vehicle>, IVehicleRepository
    {
        public VehicleRepository(ApplicationDBContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Vehicle>> GetVehiclesByCompanyIdAsync(Guid companyId)
        {
            return await _context.Vehicles
                .Include(v => v.Image)
                .Include(v => v.Tools)
                    .ThenInclude(t => t.Image)
                .Include(v => v.Employees)
                    .ThenInclude(e => e.Image)
                .Include(v => v.Employees)
                    .ThenInclude(e => e.Occupation)
                .Include(t => t.StorageItems)
                    .ThenInclude(si => si.Storage)
                        .ThenInclude(s => s.Base)
                .Include(t => t.StorageItems)
                    .ThenInclude(si => si.Storage)
                        .ThenInclude(s => s.Job)
                .Include(t => t.DefaultStorage)
                    .ThenInclude(ds => ds.Base)
                .Include(t => t.DefaultStorage)
                    .ThenInclude(ds => ds.Job)
                .Where(v => v.CompanyId == companyId)
                .ToListAsync();

        }
    }
}
