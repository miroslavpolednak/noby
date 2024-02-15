using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DomainServices.DocumentOnSAService.Api.Database.Migrations;

/// <inheritdoc />
public partial class ExternalIdExtension : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
                   name: "ExternalId",
                   table: "DocumentOnSa",
                   newName: "ExternalIdESignatures");

        migrationBuilder.AddColumn<string>(
           name: "ExternalIdSb",
           table: "DocumentOnSa",
           type: "nvarchar(50)",
           nullable: true);


        migrationBuilder.Sql("""
            GO
            UPDATE dbo.DocumentOnSa
            SET ExternalIdSb = ExternalIdESignatures
            WHERE [Source]= 2;
            GO
            UPDATE dbo.DocumentOnSa
            SET ExternalIdESignatures = NULL
            WHERE [Source]= 2;
            """);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "ExternalIdSb",
            table: "DocumentOnSa");

        migrationBuilder.RenameColumn(
            name: "ExternalIdESignatures",
            table: "DocumentOnSa",
            newName: "ExternalId");
    }
}
