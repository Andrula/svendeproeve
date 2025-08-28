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
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {

        public EmployeeRepository(ApplicationDBContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByCompanyIdAsync(Guid companyId)
        {
            return await _context.Employees
                .Include(e => e.Image)
                .Include(e => e.Occupation)
                .Include(e => e.Vehicle)
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
                .ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetAvailableEmployeesAsync(DateTime? startTime = null, DateTime? endTime = null)
        {
            var query = _dbSet.Include(e => e.StorageItems).ThenInclude(si => si.Storage);

            if (startTime.HasValue && endTime.HasValue)
            {
   
                return await query
                    .Where(e => !e.StorageItems.Any(si =>
                        si.ScheduledStart < endTime && si.ScheduledEnd > startTime))
                    .Include(e => e.Occupation)
                    .Include(e => e.Vehicle)
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
                    .ToListAsync();
            }

 
            var now = DateTime.Now;
            return await query
                .Where(e => !e.StorageItems.Any(si =>
                    si.ScheduledStart <= now && si.ScheduledEnd >= now))
                .Include(e => e.Occupation)
                .Include(e => e.Vehicle)
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
                .ToListAsync();
        }

        public override async Task<Employee?> GetByIdAsync(Guid id)
        {
            return await _dbSet
                .Include(e => e.Occupation)
                .Include(e => e.Vehicle)
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
                .FirstOrDefaultAsync(e => e.ItemId == id);
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByOccupationAsync(Guid occupationId)
        {
            return await _dbSet
                .Where(e => e.OccupationId == occupationId)
                .Include(e => e.Occupation)
                .Include(e => e.Vehicle)
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
                .ToListAsync();
        }
    }
}
