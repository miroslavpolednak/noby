using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DomainServices.DocumentArchiveService.Api.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddSendDocumentOnly : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "SEND_DOCUMENT_ONLY",
                table: "DocumentInterface",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SEND_DOCUMENT_ONLY",
                table: "DocumentInterface");
        }
    }
}
