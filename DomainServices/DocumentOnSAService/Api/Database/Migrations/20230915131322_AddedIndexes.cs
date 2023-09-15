using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DomainServices.DocumentOnSAService.Api.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddedIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_DocumentOnSa_FormId",
                table: "DocumentOnSa",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentOnSa_SalesArrangementId",
                table: "DocumentOnSa",
                column: "SalesArrangementId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DocumentOnSa_FormId",
                table: "DocumentOnSa");

            migrationBuilder.DropIndex(
                name: "IX_DocumentOnSa_SalesArrangementId",
                table: "DocumentOnSa");
        }
    }
}
