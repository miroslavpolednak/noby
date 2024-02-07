using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DomainServices.DocumentOnSAService.Api.Database.Migrations;

/// <inheritdoc />
#pragma warning disable CA1707 // Identifiers should not contain underscores
public partial class HFICH6947_Drop_SignatureMethodCode : Migration
#pragma warning restore CA1707 // Identifiers should not contain underscores
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "SignatureMethodCode",
            table: "DocumentOnSa");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "SignatureMethodCode",
            table: "DocumentOnSa",
            type: "nvarchar(15)",
            nullable: true);
    }
}
