using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Migrations
{
    /// <inheritdoc />
    public partial class DynamicStringFormatConditionDataField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DynamicStringFormatCondition",
                table: "DynamicStringFormatCondition");

            migrationBuilder.DropColumn(
                name: "DynamicStringFormatDataFieldId",
                table: "DynamicStringFormatCondition");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DynamicStringFormatCondition",
                table: "DynamicStringFormatCondition",
                columns: new[] { "DynamicStringFormatId", "DataFieldId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DynamicStringFormatCondition",
                table: "DynamicStringFormatCondition");

            migrationBuilder.AddColumn<int>(
                name: "DynamicStringFormatDataFieldId",
                table: "DynamicStringFormatCondition",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DynamicStringFormatCondition",
                table: "DynamicStringFormatCondition",
                columns: new[] { "DynamicStringFormatId", "DynamicStringFormatDataFieldId" });
        }
    }
}
