using Microsoft.EntityFrameworkCore;
using StatementProcessingService.Domain.Entities;
using StatementProcessingService.Infrastructure.Data.Configuration;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        Database.Migrate();
    }

    public DbSet<BankStatementFile> BankStatementFiles { get; set; } // Исправлено название
    public DbSet<BankStatementEntry> BankStatementEntries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfiguration(new BankStatementEntryConfiguration());
        modelBuilder.ApplyConfiguration(new BankStatementFileConfiguration());
    }
}