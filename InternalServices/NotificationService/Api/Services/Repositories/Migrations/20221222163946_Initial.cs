using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CIS.InternalServices.NotificationService.Api.Services.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmailResult",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    Channel = table.Column<int>(type: "int", nullable: false),
                    CustomId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Identity = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IdentityScheme = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DocumentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RequestTimestamp = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResultTimestamp = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Errors = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailResult", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SmsResult",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    Channel = table.Column<int>(type: "int", nullable: false),
                    CustomId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Identity = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IdentityScheme = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DocumentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RequestTimestamp = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResultTimestamp = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Errors = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountryCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmsResult", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmailResult_CustomId_Identity_IdentityScheme_DocumentId",
                table: "EmailResult",
                columns: new[] { "CustomId", "Identity", "IdentityScheme", "DocumentId" });

            migrationBuilder.CreateIndex(
                name: "IX_SmsResult_CustomId_Identity_IdentityScheme_DocumentId",
                table: "SmsResult",
                columns: new[] { "CustomId", "Identity", "IdentityScheme", "DocumentId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailResult");

            migrationBuilder.DropTable(
                name: "SmsResult");
        }
    }
}
