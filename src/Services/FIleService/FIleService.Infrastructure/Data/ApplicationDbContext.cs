using FIleService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FIleService.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
            Database.EnsureCreated();
    }
    
    public DbSet<DataRow> DataRows { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<DataRow>(entity =>
        {
            entity.ToTable("DataRows");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Date).IsRequired();
            entity.Property(e => e.LatinChars).IsRequired();
            entity.Property(e => e.CyrillicChars).IsRequired();
            entity.Property(e => e.EvenNumber).IsRequired();
            entity.Property(e => e.FloatingNumber).IsRequired();
        });
    }
}