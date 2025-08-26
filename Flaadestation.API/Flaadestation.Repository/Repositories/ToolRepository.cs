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
    public class ToolRepository : Repository<Tool>, IToolRepository
    {
        public ToolRepository(ApplicationDBContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Tool>> GetToolsByCompanyIdAsync(Guid companyId)
        {
            return await _context.Tools
                .Include(v => v.Image)
                .Include(t => t.Vehicle)
                    .ThenInclude(v => v.StorageItems)
                        .ThenInclude(si => si.Storage)
                .Include(t => t.StorageItems)
                    .ThenInclude(si => si.Storage)
                .Include(t => t.DefaultStorage)
                .Where(t => t.CompanyId == companyId)
                .ToListAsync();
        }
    }
}
