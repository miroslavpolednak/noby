using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DomainServices.DocumentArchiveService.Api.Database.Migrations
{
    /// <inheritdoc />
    public partial class CreatetAtRename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "FormInstanceInterface",
                newName: "CREATED_AT");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CREATED_AT",
                table: "FormInstanceInterface",
                newName: "CreatedAt");
        }
    }
}
