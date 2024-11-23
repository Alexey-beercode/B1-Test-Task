using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StatementProcessingService.Domain.Entities;

namespace StatementProcessingService.Infrastructure.Data.Configuration;

public class BankStatementFileConfiguration : IEntityTypeConfiguration<BankStatementFile>
{
    public void Configure(EntityTypeBuilder<BankStatementFile> builder)
    {
        builder.ToTable("BankStatementFiles");
        
        builder.HasKey(f => f.Id);
        
        builder.Property(f => f.FileName)
            .IsRequired()
            .HasMaxLength(255); 
        
        builder.Property(f => f.UploadDate)
            .IsRequired();
        
        builder.HasMany(f => f.Entries)
            .WithOne(e => e.BankStatementFile)
            .HasForeignKey(e => e.BankStatementId)
            .OnDelete(DeleteBehavior.Cascade); 
    }
}