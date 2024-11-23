using Microsoft.EntityFrameworkCore;
using StatementProcessingService.Domain.Entities;

namespace StatementProcessingService.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        if (!Database.CanConnect())
        {
            Database.Migrate();
        }
    }

    public DbSet<BankStatementFile> BankStatementsStatementFiles { get; set; }
    public DbSet<BankStatementEntry> BankStatementEntries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}