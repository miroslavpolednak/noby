using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DataService",
                columns: table => new
                {
                    DataServiceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataServiceName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataService", x => x.DataServiceId);
                });

            migrationBuilder.CreateTable(
                name: "Document",
                columns: table => new
                {
                    DocumentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Document", x => x.DocumentId);
                });

            migrationBuilder.CreateTable(
                name: "DynamicStringFormatDataField",
                columns: table => new
                {
                    DynamicStringFormatDataFieldId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FieldPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DynamicStringFormatDataField", x => x.DynamicStringFormatDataFieldId);
                });

            migrationBuilder.CreateTable(
                name: "InputParameter",
                columns: table => new
                {
                    InputParameterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InputParameterName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InputParameter", x => x.InputParameterId);
                });

            migrationBuilder.CreateTable(
                name: "DataField",
                columns: table => new
                {
                    DataFieldId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataServiceId = table.Column<int>(type: "int", nullable: false),
                    FieldPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DefaultStringFormat = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataField", x => x.DataFieldId);
                    table.ForeignKey(
                        name: "FK_DataField_DataService_DataServiceId",
                        column: x => x.DataServiceId,
                        principalTable: "DataService",
                        principalColumn: "DataServiceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DocumentSpecialDataField",
                columns: table => new
                {
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    FieldPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DataServiceId = table.Column<int>(type: "int", nullable: false),
                    TemplateFieldName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
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

            migrationBuilder.CreateTable(
                name: "DocumentDataField",
                columns: table => new
                {
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    DocumentVersion = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    DataFieldId = table.Column<int>(type: "int", nullable: false),
                    TemplateFieldName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StringFormat = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentDataField", x => new { x.DocumentId, x.DocumentVersion, x.DataFieldId });
                    table.ForeignKey(
                        name: "FK_DocumentDataField_DataField_DataFieldId",
                        column: x => x.DataFieldId,
                        principalTable: "DataField",
                        principalColumn: "DataFieldId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocumentDataField_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Document",
                        principalColumn: "DocumentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DocumentDynamicInputParameter",
                columns: table => new
                {
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    DocumentVersion = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    InputParameterId = table.Column<int>(type: "int", nullable: false),
                    TargetDataServiceId = table.Column<int>(type: "int", nullable: false),
                    SourceDataFieldId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentDynamicInputParameter", x => new { x.DocumentId, x.DocumentVersion, x.InputParameterId });
                    table.ForeignKey(
                        name: "FK_DocumentDynamicInputParameter_DataField_SourceDataFieldId",
                        column: x => x.SourceDataFieldId,
                        principalTable: "DataField",
                        principalColumn: "DataFieldId");
                    table.ForeignKey(
                        name: "FK_DocumentDynamicInputParameter_DataService_TargetDataServiceId",
                        column: x => x.TargetDataServiceId,
                        principalTable: "DataService",
                        principalColumn: "DataServiceId");
                    table.ForeignKey(
                        name: "FK_DocumentDynamicInputParameter_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Document",
                        principalColumn: "DocumentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocumentDynamicInputParameter_InputParameter_InputParameterId",
                        column: x => x.InputParameterId,
                        principalTable: "InputParameter",
                        principalColumn: "InputParameterId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DynamicStringFormat",
                columns: table => new
                {
                    DynamicStringFormatId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    DocumentVersion = table.Column<string>(type: "nvarchar(5)", nullable: false),
                    DataFieldId = table.Column<int>(type: "int", nullable: false),
                    Format = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
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
                name: "DynamicStringFormatCondition",
                columns: table => new
                {
                    DynamicStringFormatId = table.Column<int>(type: "int", nullable: false),
                    DynamicStringFormatDataFieldId = table.Column<int>(type: "int", nullable: false),
                    EqualToValue = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
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
                name: "IX_DataField_DataServiceId",
                table: "DataField",
                column: "DataServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentDataField_DataFieldId",
                table: "DocumentDataField",
                column: "DataFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentDynamicInputParameter_InputParameterId",
                table: "DocumentDynamicInputParameter",
                column: "InputParameterId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentDynamicInputParameter_SourceDataFieldId",
                table: "DocumentDynamicInputParameter",
                column: "SourceDataFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentDynamicInputParameter_TargetDataServiceId",
                table: "DocumentDynamicInputParameter",
                column: "TargetDataServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentSpecialDataField_DataServiceId",
                table: "DocumentSpecialDataField",
                column: "DataServiceId");

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
                name: "DocumentDynamicInputParameter");

            migrationBuilder.DropTable(
                name: "DocumentSpecialDataField");

            migrationBuilder.DropTable(
                name: "DynamicStringFormatCondition");

            migrationBuilder.DropTable(
                name: "InputParameter");

            migrationBuilder.DropTable(
                name: "DynamicStringFormat");

            migrationBuilder.DropTable(
                name: "DynamicStringFormatDataField");

            migrationBuilder.DropTable(
                name: "DocumentDataField");

            migrationBuilder.DropTable(
                name: "DataField");

            migrationBuilder.DropTable(
                name: "Document");

            migrationBuilder.DropTable(
                name: "DataService");
        }
    }
}
