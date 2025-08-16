using Flaadestation.Repository.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Repository.Repositories.Interfaces
{
    public interface ICompanyRepository : IRepository<Company>
    {
        Task<Company?> GetCompanyWithBasesAsync(Guid companyId);

        Task<bool> CompanyExistsByNameAsync(string name);
    }
}
