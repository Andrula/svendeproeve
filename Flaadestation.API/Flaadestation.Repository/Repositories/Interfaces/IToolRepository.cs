using Flaadestation.Repository.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Repository.Repositories.Interfaces
{
    public interface IToolRepository : IRepository<Tool>
    {
        Task<IEnumerable<Tool>> GetToolsByCompanyIdAsync(Guid companyId);
    }
}
