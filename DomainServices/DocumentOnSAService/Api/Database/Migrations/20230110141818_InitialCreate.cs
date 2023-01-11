using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DomainServices.DocumentOnSAService.Api.Database.Migrations;

/// <inheritdoc />
public partial class InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
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
            name: "IX_GeneratedFormId_HouseholdId",
            table: "GeneratedFormId",
            column: "HouseholdId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "GeneratedFormId");
    }
}
