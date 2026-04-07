using MasterService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MasterService.Infrastructure.Data;

public class MasterDbContext : DbContext
{
    public MasterDbContext(DbContextOptions<MasterDbContext> options) : base(options)
    {
    }

    public DbSet<PartMaster> PartMasters { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Mapping to the specific table name in your SQL schema
        modelBuilder.Entity<PartMaster>(entity =>
        {
            entity.ToTable("PartMaster", "dbo");
            entity.HasKey(e => e.PartId);

            // Mapping decimal precision for financial/quantity columns
            entity.Property(e => e.Quantity).HasPrecision(18, 2);
            entity.Property(e => e.UnitCost).HasPrecision(18, 2);
            entity.Property(e => e.StdCost).HasPrecision(18, 2);
            entity.Property(e => e.OnHandQuantity).HasPrecision(18, 2);

            // Ensure Varchar lengths match your SQL Schema if needed
            entity.Property(e => e.PartNumber).HasMaxLength(100);
            entity.Property(e => e.PartName).HasMaxLength(255);
        });
    }
}