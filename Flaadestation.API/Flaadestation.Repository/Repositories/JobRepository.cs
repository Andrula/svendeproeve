using Flaadestation.Repository.Database;
using Flaadestation.Repository.Database.Entities;
using Flaadestation.Repository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Repository.Repositories
{
    public class JobRepository : Repository<Job>, IJobRepository
    {
        public JobRepository(ApplicationDBContext dBContext) : base(dBContext)  {}

        public async Task<IEnumerable<Job>> GetActiveJobsAsync()
        {
            return await _context.Jobs
                    .Include(j => j.Storage)
                        .ThenInclude(s => s.StorageItems)
                    .Include(j => j.Customers)
                    .Where(j => j.ScheduledStart >= DateTime.Now && j.ScheduledEnd <= DateTime.Now)
                    .ToListAsync();
        }

        public async Task<IEnumerable<Job>> GetJobsByCompanyAsync(Guid companyId)
        {
            return await _context.Jobs
                    .Include(j => j.Storage)
                        .ThenInclude(s => s.StorageItems)
                    .Include(j => j.Customers)
                    .Where(j => j.CompanyId == companyId)
                    .ToListAsync();
        }

        public async Task<Job?> GetJobByIdAsync(Guid jobId)
        {
            return await _context.Jobs
                    .Include(j => j.Storage)
                            .ThenInclude(s => s.StorageItems)
                    .Include(j => j.Customers)
                        .FirstOrDefaultAsync(j => j.JobId == jobId);
        }

        public async Task<bool> JobExistsByTitleAsync(string title, Guid companyId)
        {
            return await _context.Jobs.AnyAsync(b => b.Title.ToLower() == title.ToLower() && b.CompanyId == companyId);
        }
    }
}
