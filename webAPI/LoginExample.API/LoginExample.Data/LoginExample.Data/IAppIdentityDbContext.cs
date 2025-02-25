using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoginExample.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace LoginExample.Data
{
    public interface IAppIdentityDbContext : IEfCoreDbContext
    {
        DbSet<AppUser> AppUsers { get; }
    }
}
