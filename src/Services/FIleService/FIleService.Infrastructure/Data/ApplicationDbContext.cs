using FIleService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FIleService.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<DataRow> DataRows { get; set; }
}