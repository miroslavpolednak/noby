using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CIS.InternalServices.NotificationService.Api.Services.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class HFICH5458DropText : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Text",
                table: "SmsResult");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "SmsResult",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
