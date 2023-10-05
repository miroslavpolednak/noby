using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CIS.InternalServices.NotificationService.Api.Services.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class HFICH8192CaseIdDocumentHash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SmsResult_CustomId_Identity_IdentityScheme_DocumentId",
                table: "SmsResult");

            migrationBuilder.DropIndex(
                name: "IX_EmailResult_CustomId_Identity_IdentityScheme_DocumentId",
                table: "EmailResult");

            migrationBuilder.AddColumn<long>(
                name: "CaseId",
                table: "SmsResult",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DocumentHash",
                table: "SmsResult",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HashAlgorithm",
                table: "SmsResult",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CaseId",
                table: "EmailResult",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DocumentHash",
                table: "EmailResult",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HashAlgorithm",
                table: "EmailResult",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SmsResult_CustomId_Identity_IdentityScheme_DocumentId_CaseId",
                table: "SmsResult",
                columns: new[] { "CustomId", "Identity", "IdentityScheme", "DocumentId", "CaseId" });

            migrationBuilder.CreateIndex(
                name: "IX_EmailResult_CustomId_Identity_IdentityScheme_DocumentId_CaseId",
                table: "EmailResult",
                columns: new[] { "CustomId", "Identity", "IdentityScheme", "DocumentId", "CaseId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SmsResult_CustomId_Identity_IdentityScheme_DocumentId_CaseId",
                table: "SmsResult");

            migrationBuilder.DropIndex(
                name: "IX_EmailResult_CustomId_Identity_IdentityScheme_DocumentId_CaseId",
                table: "EmailResult");

            migrationBuilder.DropColumn(
                name: "CaseId",
                table: "SmsResult");

            migrationBuilder.DropColumn(
                name: "DocumentHash",
                table: "SmsResult");

            migrationBuilder.DropColumn(
                name: "HashAlgorithm",
                table: "SmsResult");

            migrationBuilder.DropColumn(
                name: "CaseId",
                table: "EmailResult");

            migrationBuilder.DropColumn(
                name: "DocumentHash",
                table: "EmailResult");

            migrationBuilder.DropColumn(
                name: "HashAlgorithm",
                table: "EmailResult");

            migrationBuilder.CreateIndex(
                name: "IX_SmsResult_CustomId_Identity_IdentityScheme_DocumentId",
                table: "SmsResult",
                columns: new[] { "CustomId", "Identity", "IdentityScheme", "DocumentId" });

            migrationBuilder.CreateIndex(
                name: "IX_EmailResult_CustomId_Identity_IdentityScheme_DocumentId",
                table: "EmailResult",
                columns: new[] { "CustomId", "Identity", "IdentityScheme", "DocumentId" });
        }
    }
}
