using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Migrations
{
    /// <inheritdoc />
    public partial class EasForm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Format",
                table: "DynamicStringFormat",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.CreateTable(
                name: "EasFormType",
                columns: table => new
                {
                    EasFormTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EasFormTypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EasFormType", x => x.EasFormTypeId);
                });

            migrationBuilder.CreateTable(
                name: "EasRequestType",
                columns: table => new
                {
                    EasRequestTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EasRequestTypeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EasRequestType", x => x.EasRequestTypeId);
                });

            migrationBuilder.CreateTable(
                name: "EasFormDataField",
                columns: table => new
                {
                    EasFormDataFieldId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EasRequestTypeId = table.Column<int>(type: "int", nullable: false),
                    DataFieldId = table.Column<int>(type: "int", nullable: false),
                    EasFormTypeId = table.Column<int>(type: "int", nullable: false),
                    JsonPropertyName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EasFormDataField", x => x.EasFormDataFieldId);
                    table.ForeignKey(
                        name: "FK_EasFormDataField_DataField_DataFieldId",
                        column: x => x.DataFieldId,
                        principalTable: "DataField",
                        principalColumn: "DataFieldId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EasFormDataField_EasFormType_EasFormTypeId",
                        column: x => x.EasFormTypeId,
                        principalTable: "EasFormType",
                        principalColumn: "EasFormTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EasFormDataField_EasRequestType_EasRequestTypeId",
                        column: x => x.EasRequestTypeId,
                        principalTable: "EasRequestType",
                        principalColumn: "EasRequestTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EasFormDataField_DataFieldId",
                table: "EasFormDataField",
                column: "DataFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_EasFormDataField_EasFormTypeId",
                table: "EasFormDataField",
                column: "EasFormTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EasFormDataField_EasRequestTypeId_EasFormTypeId_JsonPropertyName",
                table: "EasFormDataField",
                columns: new[] { "EasRequestTypeId", "EasFormTypeId", "JsonPropertyName" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EasFormDataField");

            migrationBuilder.DropTable(
                name: "EasFormType");

            migrationBuilder.DropTable(
                name: "EasRequestType");

            migrationBuilder.AlterColumn<string>(
                name: "Format",
                table: "DynamicStringFormat",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
