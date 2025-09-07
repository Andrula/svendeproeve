using Flaadestation.Repository.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Repository.Repositories.Interfaces
{
    public interface IJobRepository : IRepository<Job>
    {
        Task<IEnumerable<Job>> GetActiveJobsAsync();
        Task<Job?> GetJobByIdAsync(Guid jobId);
        Task<bool> JobExistsByTitleAsync(string name, Guid companyId);
        Task<IEnumerable<Job>> GetJobsByCompanyAsync(Guid companyId);
    }
}
