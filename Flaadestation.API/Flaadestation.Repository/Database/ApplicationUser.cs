using Flaadestation.Repository.Database.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Repository.Database
{
    public class ApplicationUser : IdentityUser
    {
        public Guid CompanyId { get; set; }

        public Company? Company { get; set; }
    }
}
