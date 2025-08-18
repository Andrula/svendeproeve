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
    public class OccupationRepository : Repository<Occupation>, IOccupationRepository
    {
        public OccupationRepository(ApplicationDBContext dbContext) : base(dbContext)
        {
            
        }

        public async Task<bool> OccupationExistsByNameAsync(string name)
        {
            return await _dbSet
              .AnyAsync(c => c.Name.ToLower() == name.ToLower());
        }
    }
}
