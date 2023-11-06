using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DomainServices.DocumentOnSAService.Api.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddedIsCustomerPreviewSendingAllowed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCustomerPreviewSendingAllowed",
                table: "DocumentOnSa",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCustomerPreviewSendingAllowed",
                table: "DocumentOnSa");
        }
    }
}
