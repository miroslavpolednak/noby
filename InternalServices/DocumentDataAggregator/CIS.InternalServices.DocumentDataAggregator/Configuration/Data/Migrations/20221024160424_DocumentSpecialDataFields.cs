using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Migrations
{
    public partial class DocumentSpecialDataFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DocumentSpecialDataField",
                columns: table => new
                {
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    FieldPath = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DataServiceId = table.Column<int>(type: "int", nullable: false),
                    TemplateFieldName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentSpecialDataField", x => new { x.DocumentId, x.FieldPath });
                    table.ForeignKey(
                        name: "FK_DocumentSpecialDataField_DataService_DataServiceId",
                        column: x => x.DataServiceId,
                        principalTable: "DataService",
                        principalColumn: "DataServiceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocumentSpecialDataField_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Document",
                        principalColumn: "DocumentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentSpecialDataField_DataServiceId",
                table: "DocumentSpecialDataField",
                column: "DataServiceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentSpecialDataField");
        }
    }
}
