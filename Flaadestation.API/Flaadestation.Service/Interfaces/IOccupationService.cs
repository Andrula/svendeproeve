using Flaadestation.Repository.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Service.Interfaces
{
    public interface IOccupationService
    {
        Task<Occupation> GetOccupationByIdAsync(Guid id);
        Task<IEnumerable<Occupation>> GetAllOccupationsAsync();
        Task<Occupation> CreateOccupationAsync(Occupation occupation);
        Task<Occupation?> UpdateOccupationAsync(Guid id, Occupation occupation);
        Task<bool> DeleteOccupationAsync(Guid id);
        Task<bool> IsOccupationNameAvailableAsync(string name);
    }
}
