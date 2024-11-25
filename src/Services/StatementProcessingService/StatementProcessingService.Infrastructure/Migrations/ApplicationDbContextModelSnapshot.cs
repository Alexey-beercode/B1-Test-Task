﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace StatementProcessingService.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("StatementProcessingService.Domain.Entities.BankStatementEntry", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AccountNumber")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)");

                    b.Property<Guid>("BankStatementId")
                        .HasMaxLength(255)
                        .HasColumnType("uuid");

                    b.Property<decimal>("FinalBalanceActive")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("FinalBalancePassive")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("InitialBalanceActive")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("InitialBalancePassive")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("TurnoverCredit")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("TurnoverDebit")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("BankStatementId");

                    b.ToTable("BankStatementEntries", (string)null);
                });

            modelBuilder.Entity("StatementProcessingService.Domain.Entities.BankStatementFile", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<DateTime>("UploadDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("BankStatementFiles", (string)null);
                });

            modelBuilder.Entity("StatementProcessingService.Domain.Entities.BankStatementEntry", b =>
                {
                    b.HasOne("StatementProcessingService.Domain.Entities.BankStatementFile", "BankStatementFile")
                        .WithMany("Entries")
                        .HasForeignKey("BankStatementId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BankStatementFile");
                });

            modelBuilder.Entity("StatementProcessingService.Domain.Entities.BankStatementFile", b =>
                {
                    b.Navigation("Entries");
                });
#pragma warning restore 612, 618
        }
    }
}
