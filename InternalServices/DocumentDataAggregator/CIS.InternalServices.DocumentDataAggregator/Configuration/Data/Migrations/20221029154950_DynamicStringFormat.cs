using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Migrations
{
    public partial class DynamicStringFormat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DynamicStringFormat",
                columns: table => new
                {
                    DynamicStringFormatId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    DocumentVersion = table.Column<int>(type: "int", nullable: false),
                    DataFieldId = table.Column<int>(type: "int", nullable: false),
                    Format = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DynamicStringFormat", x => x.DynamicStringFormatId);
                    table.ForeignKey(
                        name: "FK_DynamicStringFormat_DocumentDataField_DocumentId_DocumentVersion_DataFieldId",
                        columns: x => new { x.DocumentId, x.DocumentVersion, x.DataFieldId },
                        principalTable: "DocumentDataField",
                        principalColumns: new[] { "DocumentId", "DocumentVersion", "DataFieldId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DynamicStringFormatDataField",
                columns: table => new
                {
                    DynamicStringFormatDataFieldId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FieldPath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DynamicStringFormatDataField", x => x.DynamicStringFormatDataFieldId);
                });

            migrationBuilder.CreateTable(
                name: "DynamicStringFormatCondition",
                columns: table => new
                {
                    DynamicStringFormatId = table.Column<int>(type: "int", nullable: false),
                    DynamicStringFormatDataFieldId = table.Column<int>(type: "int", nullable: false),
                    EqualToValue = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DynamicStringFormatCondition", x => new { x.DynamicStringFormatId, x.DynamicStringFormatDataFieldId });
                    table.ForeignKey(
                        name: "FK_DynamicStringFormatCondition_DynamicStringFormat_DynamicStringFormatId",
                        column: x => x.DynamicStringFormatId,
                        principalTable: "DynamicStringFormat",
                        principalColumn: "DynamicStringFormatId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DynamicStringFormatCondition_DynamicStringFormatDataField_DynamicStringFormatDataFieldId",
                        column: x => x.DynamicStringFormatDataFieldId,
                        principalTable: "DynamicStringFormatDataField",
                        principalColumn: "DynamicStringFormatDataFieldId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DynamicStringFormat_DocumentId_DocumentVersion_DataFieldId",
                table: "DynamicStringFormat",
                columns: new[] { "DocumentId", "DocumentVersion", "DataFieldId" });

            migrationBuilder.CreateIndex(
                name: "IX_DynamicStringFormatCondition_DynamicStringFormatDataFieldId",
                table: "DynamicStringFormatCondition",
                column: "DynamicStringFormatDataFieldId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DynamicStringFormatCondition");

            migrationBuilder.DropTable(
                name: "DynamicStringFormat");

            migrationBuilder.DropTable(
                name: "DynamicStringFormatDataField");
        }
    }
}
