using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DomainServices.DocumentOnSAService.Api.Database.Migrations;

/// <inheritdoc />
#pragma warning disable CA1707 // Identifiers should not contain underscores
public partial class HFICH6054_HFICH5974_SignatureProps : Migration
#pragma warning restore CA1707 // Identifiers should not contain underscores
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "IsDocumentArchived",
            table: "DocumentOnSa",
            newName: "IsArchived");

        migrationBuilder.AddColumn<string>(
            name: "ExternalId",
            table: "DocumentOnSa",
            type: "nvarchar(50)",
            nullable: true);

        migrationBuilder.AddColumn<int>(
            name: "Source",
            table: "DocumentOnSa",
            type: "int",
            nullable: false,
            defaultValue: 1);

        migrationBuilder.CreateTable(
            name: "EArchivIdsLinked",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                DocumentOnSAId = table.Column<int>(type: "int", nullable: false),
                EArchivId = table.Column<string>(type: "nvarchar(50)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_EArchivIdsLinked", x => x.Id);
                table.ForeignKey(
                    name: "FK_EArchivIdsLinked_DocumentOnSa_DocumentOnSAId",
                    column: x => x.DocumentOnSAId,
                    principalTable: "DocumentOnSa",
                    principalColumn: "DocumentOnSAId",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_EArchivIdsLinked_DocumentOnSAId",
            table: "EArchivIdsLinked",
            column: "DocumentOnSAId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "EArchivIdsLinked");

        migrationBuilder.DropColumn(
            name: "ExternalId",
            table: "DocumentOnSa");

        migrationBuilder.DropColumn(
            name: "Source",
            table: "DocumentOnSa");

        migrationBuilder.RenameColumn(
            name: "IsArchived",
            table: "DocumentOnSa",
            newName: "IsDocumentArchived");
    }
}
