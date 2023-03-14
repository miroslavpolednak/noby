using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DomainServices.DocumentOnSAService.Api.Database.Migrations;

/// <inheritdoc />
public partial class IsFinalAndMandatories : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "SignatureMethodCode",
            table: "DocumentOnSa",
            type: "nvarchar(15)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(15)");

        migrationBuilder.AlterColumn<int>(
            name: "HouseholdId",
            table: "DocumentOnSa",
            type: "int",
            nullable: true,
            oldClrType: typeof(int),
            oldType: "int");

        migrationBuilder.AddColumn<bool>(
            name: "IsFinal",
            table: "DocumentOnSa",
            type: "bit",
            nullable: false,
            defaultValue: false);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "IsFinal",
            table: "DocumentOnSa");

        migrationBuilder.AlterColumn<string>(
            name: "SignatureMethodCode",
            table: "DocumentOnSa",
            type: "nvarchar(15)",
            nullable: false,
            defaultValue: "",
            oldClrType: typeof(string),
            oldType: "nvarchar(15)",
            oldNullable: true);

        migrationBuilder.AlterColumn<int>(
            name: "HouseholdId",
            table: "DocumentOnSa",
            type: "int",
            nullable: false,
            defaultValue: 0,
            oldClrType: typeof(int),
            oldType: "int",
            oldNullable: true);
    }
}
