using Flaadestation.Repository.Database;
using Flaadestation.Repository.Database.Entities;
using Flaadestation.Repository.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Repository.Repositories
{
    public class JobRepository : Repository<Job>, IJobRepository
    {
        public JobRepository(ApplicationDBContext dBContext) : base(dBContext)  {}

        public Task<IEnumerable<Job>> GetActiveJobsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Job>> GetJobsByCompanyAsync(Guid companyId)
        {
            throw new NotImplementedException();
        }
    }
}
