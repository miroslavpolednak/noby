using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DomainServices.DocumentArchiveService.Api.Database.Migrations
{
    /// <inheritdoc />
    public partial class ColumnRenaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DATUM_PRIJETI",
                table: "DocumentInterface",
                newName: "CREATED_ON");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CREATED_ON",
                table: "DocumentInterface",
                newName: "DATUM_PRIJETI");
        }
    }
}
