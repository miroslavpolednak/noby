using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DomainServices.DocumentOnSAService.Api.Database.Migrations;

/// <inheritdoc />
public partial class HFICH8142_AddEaCodeMainId : Migration
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
