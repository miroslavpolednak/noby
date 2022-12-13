using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Migrations
{
    /// <inheritdoc />
    public partial class EasFormSpecialFieldPK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EasFormSpecialDataField",
                table: "EasFormSpecialDataField");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EasFormSpecialDataField",
                table: "EasFormSpecialDataField",
                columns: new[] { "EasRequestTypeId", "JsonPropertyName", "EasFormTypeId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EasFormSpecialDataField",
                table: "EasFormSpecialDataField");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EasFormSpecialDataField",
                table: "EasFormSpecialDataField",
                columns: new[] { "EasRequestTypeId", "JsonPropertyName" });
        }
    }
}
