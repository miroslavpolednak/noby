using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DomainServices.DocumentOnSAService.Api.Database.Migrations;

/// <inheritdoc />
 #pragma warning disable CA1707 // Identifiers should not contain underscores
public partial class HFICH7885_AddedTaskIdSb : Migration
#pragma warning restore CA1707 // Identifiers should not contain underscores
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "TaskIdSb",
            table: "DocumentOnSa",
            type: "int",
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "TaskIdSb",
            table: "DocumentOnSa");
    }
}
