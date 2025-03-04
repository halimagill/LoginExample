using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoginExample.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace LoginExample.Data
{
    public class AppIdentityDbContext : IdentityDbContext<AppUser>, IAppIdentityDbContext
    {
        private readonly string _connectionString;
        public AppIdentityDbContext()
        {
        }

        public AppIdentityDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        //public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
        //{

        //}

        public DbSet<AppUser>? AppUsers { get; set; }

        public Task<int> SaveChangesOnDbContextAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);

            //.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            //.Build();

            //    var builder = new ConfigurationBuilder();
            //    builder.AddConfiguration(new IConfiguration)
            //       .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            //       .AddJsonFile("appsettings.json")
            //       .Build();
            //    optionsBuilder.UseSqlServer(_connectionString);

            //    var build = new ConfigurationBuilder();
            //    build.SetBasePath(Directory.GetCurrentDirectory()); 
            //    build.AddJsonFile(CONFIG_FILE, true, true);
            //    Configs = build.Build();

            // optionsBuilder.UseSqlServer("Server=PCSONIC;Database=LoginExample;Integrated Security=true;MultipleActiveResultSets=true;TrustServerCertificate=True;");
        }
    }
}
