using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DomainServices.DocumentOnSAService.Api.Database.Migrations
{
    /// <inheritdoc />
    public partial class HFICH3917_json_obj : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CaseId",
                table: "DocumentOnSa",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CustomerOnSAId1",
                table: "DocumentOnSa",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CustomerOnSAId2",
                table: "DocumentOnSa",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPreviewSentToCustomer",
                table: "DocumentOnSa",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TaskId",
                table: "DocumentOnSa",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SigningIdentity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentOnSAId = table.Column<int>(type: "int", nullable: false),
                    SigningIdentityJson = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SigningIdentity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SigningIdentity_DocumentOnSa_DocumentOnSAId",
                        column: x => x.DocumentOnSAId,
                        principalTable: "DocumentOnSa",
                        principalColumn: "DocumentOnSAId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SigningIdentity_DocumentOnSAId",
                table: "SigningIdentity",
                column: "DocumentOnSAId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SigningIdentity");

            migrationBuilder.DropColumn(
                name: "CaseId",
                table: "DocumentOnSa");

            migrationBuilder.DropColumn(
                name: "CustomerOnSAId1",
                table: "DocumentOnSa");

            migrationBuilder.DropColumn(
                name: "CustomerOnSAId2",
                table: "DocumentOnSa");

            migrationBuilder.DropColumn(
                name: "IsPreviewSentToCustomer",
                table: "DocumentOnSa");

            migrationBuilder.DropColumn(
                name: "TaskId",
                table: "DocumentOnSa");
        }
    }
}
