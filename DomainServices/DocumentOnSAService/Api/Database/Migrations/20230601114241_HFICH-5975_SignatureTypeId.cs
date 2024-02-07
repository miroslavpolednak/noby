using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DomainServices.DocumentOnSAService.Api.Database.Migrations;

/// <inheritdoc />
#pragma warning disable CA1707 // Identifiers should not contain underscores
public partial class HFICH5975_SignatureTypeId : Migration
#pragma warning restore CA1707 // Identifiers should not contain underscores
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "Data",
            table: "DocumentOnSa",
            type: "nvarchar(MAX)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(MAX)");

        migrationBuilder.AddColumn<int>(
            name: "SignatureTypeId",
            table: "DocumentOnSa",
            type: "int",
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "SignatureTypeId",
            table: "DocumentOnSa");

        migrationBuilder.AlterColumn<string>(
            name: "Data",
            table: "DocumentOnSa",
            type: "nvarchar(MAX)",
            nullable: false,
            defaultValue: "",
            oldClrType: typeof(string),
            oldType: "nvarchar(MAX)",
            oldNullable: true);
    }
}
