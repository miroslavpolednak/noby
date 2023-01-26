using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DomainServices.DocumentOnSAService.Api.Database.Migrations;

/// <inheritdoc />
public partial class InitDb : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "DocumentOnSa",
            columns: table => new
            {
                DocumentOnSAId = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                DocumentTypeId = table.Column<int>(type: "int", nullable: false),
                DocumentTemplateVersionId = table.Column<int>(type: "int", nullable: true),
                FormId = table.Column<string>(type: "nvarchar(15)", nullable: false),
                EArchivId = table.Column<string>(type: "nvarchar(50)", nullable: true),
                DmsxId = table.Column<string>(type: "nvarchar(50)", nullable: true),
                SalesArrangementId = table.Column<int>(type: "int", nullable: false),
                HouseholdId = table.Column<int>(type: "int", nullable: false),
                IsValid = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                IsSigned = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                IsDocumentArchived = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                SignatureMethodCode = table.Column<string>(type: "nvarchar(15)", nullable: false),
                SignatureDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                SignatureConfirmedBy = table.Column<int>(type: "int", nullable: true),
                CreatedUserName = table.Column<string>(type: "nvarchar(50)", nullable: true),
                CreatedUserId = table.Column<int>(type: "int", nullable: true),
                CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                Data = table.Column<string>(type: "nvarchar(MAX)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_DocumentOnSa", x => x.DocumentOnSAId);
            });

        migrationBuilder.CreateTable(
            name: "GeneratedFormId",
            columns: table => new
            {
                Id = table.Column<long>(type: "bigint", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                HouseholdId = table.Column<int>(type: "int", nullable: true),
                Version = table.Column<short>(type: "smallint", nullable: false),
                IsFormIdFinal = table.Column<bool>(type: "bit", nullable: false),
                TargetSystem = table.Column<string>(type: "nvarchar(2)", nullable: false, defaultValue: "N")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_GeneratedFormId", x => x.Id);
            });

        migrationBuilder.CreateIndex(
            name: "IX_DocumentOnSa_SalesArrangementId",
            table: "DocumentOnSa",
            column: "SalesArrangementId");

        migrationBuilder.CreateIndex(
            name: "IX_GeneratedFormId_HouseholdId",
            table: "GeneratedFormId",
            column: "HouseholdId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "DocumentOnSa");

        migrationBuilder.DropTable(
            name: "GeneratedFormId");
    }
}
