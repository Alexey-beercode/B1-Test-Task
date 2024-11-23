using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StatementProcessingService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BankStatementFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FileName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    UploadDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankStatementFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BankStatementEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BankStatementId = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    InitialBalanceActive = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    InitialBalancePassive = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    TurnoverDebit = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    TurnoverCredit = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    FinalBalanceActive = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    FinalBalancePassive = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankStatementEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankStatementEntries_BankStatementFiles_BankStatementId",
                        column: x => x.BankStatementId,
                        principalTable: "BankStatementFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankStatementEntries_BankStatementId",
                table: "BankStatementEntries",
                column: "BankStatementId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BankStatementEntries");

            migrationBuilder.DropTable(
                name: "BankStatementFiles");
        }
    }
}
