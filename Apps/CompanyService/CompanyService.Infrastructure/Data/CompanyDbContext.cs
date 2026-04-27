using CompanyService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CompanyService.Infrastructure.Data
{
    public class CompanyDbContext : DbContext
    {
        public CompanyDbContext(DbContextOptions<CompanyDbContext> options) : base(options) { }
        public DbSet<companymaster> Companies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Map to the specific schema and table you created in DBeaver
            modelBuilder.Entity<companymaster>().ToTable("companymaster", "dbo");

            // Ensure ID is recognized as the Primary Key
            modelBuilder.Entity<companymaster>().HasKey(c => c.companyid);
        }
    }
}
