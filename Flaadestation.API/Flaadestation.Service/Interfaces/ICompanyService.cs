using Flaadestation.Repository.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Service.Interfaces
{
    public interface ICompanyService
    {
        Task<Company?> GetCompanyByIdAsync(Guid id);
        Task<Company> CreateCompanyAsync(Company company);
        Task<Company?> UpdateCompanyAsync(Guid id, Company company);
        Task<bool> DeleteCompanyAsync(Guid id);
        Task<bool> IsCompanyNameAvailableAsync(string name);
    }
}
