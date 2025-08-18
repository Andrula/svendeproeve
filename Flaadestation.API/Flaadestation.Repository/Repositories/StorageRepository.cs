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
    public class StorageRepository : Repository<Storage>, IStorageRepository
    {
        public StorageRepository(ApplicationDBContext dBContext) : base(dBContext) { }

    }
}
