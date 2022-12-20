using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Migrations
{
    /// <inheritdoc />
    public partial class DocumentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DocumentTable",
                columns: table => new
                {
                    DocumentTableId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    DocumentVersion = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    DataFieldId = table.Column<int>(type: "int", nullable: false),
                    AcroFieldPlaceholderName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTable", x => x.DocumentTableId);
                    table.ForeignKey(
                        name: "FK_DocumentTable_DataField_DataFieldId",
                        column: x => x.DataFieldId,
                        principalTable: "DataField",
                        principalColumn: "DataFieldId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocumentTable_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Document",
                        principalColumn: "DocumentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DocumentTableColumn",
                columns: table => new
                {
                    DocumentTableId = table.Column<int>(type: "int", nullable: false),
                    FieldPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    WidthPercentage = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTableColumn", x => new { x.DocumentTableId, x.FieldPath });
                    table.ForeignKey(
                        name: "FK_DocumentTableColumn_DocumentTable_DocumentTableId",
                        column: x => x.DocumentTableId,
                        principalTable: "DocumentTable",
                        principalColumn: "DocumentTableId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTable_DataFieldId",
                table: "DocumentTable",
                column: "DataFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTable_DocumentId",
                table: "DocumentTable",
                column: "DocumentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentTableColumn");

            migrationBuilder.DropTable(
                name: "DocumentTable");
        }
    }
}
