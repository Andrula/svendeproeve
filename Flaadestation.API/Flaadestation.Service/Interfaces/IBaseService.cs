using Flaadestation.Repository.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Service.Interfaces
{
    public interface IBaseService
    {
        Task<IEnumerable<Base>> GetBasesByCompanyAsync(Guid companyId);
        Task<Base?> GetBaseByIdAsync(Guid id);
        Task<Base?> GetBaseWithStorageAsync(Guid baseId);
        Task<Base> CreateBaseAsync(Base baseEntity);
        Task<Base?> UpdateBaseAsync(Guid id, string name);
        Task<bool> DeleteBaseAsync(Guid id);
        Task<bool> IsBaseNameAvailableAsync(string name, Guid companyId);
    }
}
