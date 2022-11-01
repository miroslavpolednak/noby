using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Migrations
{
    public partial class DocumentSpecialField_StringFormat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StringFormat",
                table: "DocumentSpecialDataField",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StringFormat",
                table: "DocumentSpecialDataField");
        }
    }
}
