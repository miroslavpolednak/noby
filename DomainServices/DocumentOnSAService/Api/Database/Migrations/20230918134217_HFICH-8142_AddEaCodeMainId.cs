using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DomainServices.DocumentOnSAService.Api.Database.Migrations;

/// <inheritdoc />
#pragma warning disable CA1707 // Identifiers should not contain underscores
public partial class HFICH8142_AddEaCodeMainId : Migration
#pragma warning restore CA1707 // Identifiers should not contain underscores
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "EACodeMainId",
            table: "DocumentOnSa",
            type: "int",
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "EACodeMainId",
            table: "DocumentOnSa");
    }
}
