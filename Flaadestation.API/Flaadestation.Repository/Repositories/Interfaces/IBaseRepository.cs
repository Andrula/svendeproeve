using Flaadestation.Repository.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Repository.Repositories.Interfaces
{
    public interface IBaseRepository : IRepository<Base>
    {
        Task<IEnumerable<Base>> GetBasesByCompanyAsync(Guid companyId);
    }
}
