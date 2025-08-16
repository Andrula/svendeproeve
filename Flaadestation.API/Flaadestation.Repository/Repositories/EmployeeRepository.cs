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

        public override async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _dbSet
                .Include(e => e.Occupation)
                .Include(e => e.Vehicle)
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
                    .ToListAsync();
            }

 
            var now = DateTime.Now;
            return await query
                .Where(e => !e.StorageItems.Any(si =>
                    si.ScheduledStart <= now && si.ScheduledEnd >= now))
                .Include(e => e.Occupation)
                .Include(e => e.Vehicle)
                .ToListAsync();
        }

        public override async Task<Employee?> GetByIdAsync(Guid id)
        {
            return await _dbSet
                .Include(e => e.Occupation)
                .Include(e => e.Vehicle)
                .FirstOrDefaultAsync(e => e.ItemId == id);
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByOccupationAsync(Guid occupationId)
        {
            return await _dbSet
                .Where(e => e.OccupationId == occupationId)
                .Include(e => e.Occupation)
                .Include(e => e.Vehicle)
                .ToListAsync();
        }
    }
}
