using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StatementProcessingService.Domain.Entities;

namespace StatementProcessingService.Infrastructure.Data.Configuration;

public class BankStatementEntryConfiguration : IEntityTypeConfiguration<BankStatementEntry>
{
    public void Configure(EntityTypeBuilder<BankStatementEntry> builder)
    {
        builder.ToTable("BankStatementEntries");
        
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.BankStatementId)
            .IsRequired();
        
        builder.Property(e => e.AccountNumber)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(e => e.InitialBalanceActive)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(e => e.InitialBalancePassive)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(e => e.TurnoverDebit)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(e => e.TurnoverCredit)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(e => e.FinalBalanceActive)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(e => e.FinalBalancePassive)
            .HasColumnType("decimal(18,2)")
            .IsRequired();
        
        builder.HasOne(e => e.BankStatementFile)
            .WithMany(f => f.Entries)
            .HasForeignKey(e => e.BankStatementId)
            .OnDelete(DeleteBehavior.Cascade); 
    }
}