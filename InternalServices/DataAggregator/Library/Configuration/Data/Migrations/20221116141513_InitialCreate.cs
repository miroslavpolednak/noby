using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DataService",
                columns: table => new
                {
                    DataServiceId = table.Column<int>(type: "int", nullable: false),
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
                name: "EasFormType",
                columns: table => new
                {
                    EasFormTypeId = table.Column<int>(type: "int", nullable: false),
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
                    EasRequestTypeId = table.Column<int>(type: "int", nullable: false),
                    EasRequestTypeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EasRequestType", x => x.EasRequestTypeId);
                });

            migrationBuilder.CreateTable(
                name: "InputParameter",
                columns: table => new
                {
                    InputParameterId = table.Column<int>(type: "int", nullable: false),
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
                    AcroFieldName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DataServiceId = table.Column<int>(type: "int", nullable: false),
                    FieldPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    StringFormat = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentSpecialDataField", x => new { x.DocumentId, x.AcroFieldName });
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
                name: "EasFormSpecialDataField",
                columns: table => new
                {
                    EasRequestTypeId = table.Column<int>(type: "int", nullable: false),
                    JsonPropertyName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DataServiceId = table.Column<int>(type: "int", nullable: false),
                    EasFormTypeId = table.Column<int>(type: "int", nullable: false),
                    FieldPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EasFormSpecialDataField", x => new { x.EasRequestTypeId, x.JsonPropertyName });
                    table.ForeignKey(
                        name: "FK_EasFormSpecialDataField_DataService_DataServiceId",
                        column: x => x.DataServiceId,
                        principalTable: "DataService",
                        principalColumn: "DataServiceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EasFormSpecialDataField_EasFormType_EasFormTypeId",
                        column: x => x.EasFormTypeId,
                        principalTable: "EasFormType",
                        principalColumn: "EasFormTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EasFormSpecialDataField_EasRequestType_EasRequestTypeId",
                        column: x => x.EasRequestTypeId,
                        principalTable: "EasRequestType",
                        principalColumn: "EasRequestTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DocumentDataField",
                columns: table => new
                {
                    DocumentDataFieldId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    DocumentVersion = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    DataFieldId = table.Column<int>(type: "int", nullable: false),
                    AcroFieldName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StringFormat = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DefaultTextIfNull = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentDataField", x => x.DocumentDataFieldId);
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
                    table.PrimaryKey("PK_DocumentDynamicInputParameter", x => new { x.DocumentId, x.DocumentVersion, x.InputParameterId, x.TargetDataServiceId });
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

            migrationBuilder.CreateTable(
                name: "EasFormDynamicInputParameter",
                columns: table => new
                {
                    EasRequestTypeId = table.Column<int>(type: "int", nullable: false),
                    InputParameterId = table.Column<int>(type: "int", nullable: false),
                    TargetDataServiceId = table.Column<int>(type: "int", nullable: false),
                    SourceDataFieldId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EasFormDynamicInputParameter", x => new { x.EasRequestTypeId, x.InputParameterId, x.TargetDataServiceId });
                    table.ForeignKey(
                        name: "FK_EasFormDynamicInputParameter_DataField_SourceDataFieldId",
                        column: x => x.SourceDataFieldId,
                        principalTable: "DataField",
                        principalColumn: "DataFieldId");
                    table.ForeignKey(
                        name: "FK_EasFormDynamicInputParameter_DataService_TargetDataServiceId",
                        column: x => x.TargetDataServiceId,
                        principalTable: "DataService",
                        principalColumn: "DataServiceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EasFormDynamicInputParameter_EasRequestType_EasRequestTypeId",
                        column: x => x.EasRequestTypeId,
                        principalTable: "EasRequestType",
                        principalColumn: "EasRequestTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EasFormDynamicInputParameter_InputParameter_InputParameterId",
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
                    DocumentDataFieldId = table.Column<int>(type: "int", nullable: false),
                    Format = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DynamicStringFormat", x => x.DynamicStringFormatId);
                    table.ForeignKey(
                        name: "FK_DynamicStringFormat_DocumentDataField_DocumentDataFieldId",
                        column: x => x.DocumentDataFieldId,
                        principalTable: "DocumentDataField",
                        principalColumn: "DocumentDataFieldId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DynamicStringFormatCondition",
                columns: table => new
                {
                    DynamicStringFormatId = table.Column<int>(type: "int", nullable: false),
                    DynamicStringFormatDataFieldId = table.Column<int>(type: "int", nullable: false),
                    EqualToValue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DataFieldId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DynamicStringFormatCondition", x => new { x.DynamicStringFormatId, x.DynamicStringFormatDataFieldId });
                    table.ForeignKey(
                        name: "FK_DynamicStringFormatCondition_DataField_DataFieldId",
                        column: x => x.DataFieldId,
                        principalTable: "DataField",
                        principalColumn: "DataFieldId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DynamicStringFormatCondition_DynamicStringFormat_DynamicStringFormatId",
                        column: x => x.DynamicStringFormatId,
                        principalTable: "DynamicStringFormat",
                        principalColumn: "DynamicStringFormatId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DataField_DataServiceId",
                table: "DataField",
                column: "DataServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_DataField_FieldPath",
                table: "DataField",
                column: "FieldPath",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentDataField_DataFieldId",
                table: "DocumentDataField",
                column: "DataFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentDataField_DocumentId_DocumentVersion_AcroFieldName",
                table: "DocumentDataField",
                columns: new[] { "DocumentId", "DocumentVersion", "AcroFieldName" });

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
                name: "IX_DynamicStringFormat_DocumentDataFieldId",
                table: "DynamicStringFormat",
                column: "DocumentDataFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_DynamicStringFormatCondition_DataFieldId",
                table: "DynamicStringFormatCondition",
                column: "DataFieldId");

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

            migrationBuilder.CreateIndex(
                name: "IX_EasFormDynamicInputParameter_InputParameterId",
                table: "EasFormDynamicInputParameter",
                column: "InputParameterId");

            migrationBuilder.CreateIndex(
                name: "IX_EasFormDynamicInputParameter_SourceDataFieldId",
                table: "EasFormDynamicInputParameter",
                column: "SourceDataFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_EasFormDynamicInputParameter_TargetDataServiceId",
                table: "EasFormDynamicInputParameter",
                column: "TargetDataServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_EasFormSpecialDataField_DataServiceId",
                table: "EasFormSpecialDataField",
                column: "DataServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_EasFormSpecialDataField_EasFormTypeId",
                table: "EasFormSpecialDataField",
                column: "EasFormTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentDynamicInputParameter");

            migrationBuilder.DropTable(
                name: "DocumentSpecialDataField");

            migrationBuilder.DropTable(
                name: "DynamicStringFormatCondition");

            migrationBuilder.DropTable(
                name: "EasFormDataField");

            migrationBuilder.DropTable(
                name: "EasFormDynamicInputParameter");

            migrationBuilder.DropTable(
                name: "EasFormSpecialDataField");

            migrationBuilder.DropTable(
                name: "DynamicStringFormat");

            migrationBuilder.DropTable(
                name: "InputParameter");

            migrationBuilder.DropTable(
                name: "EasFormType");

            migrationBuilder.DropTable(
                name: "EasRequestType");

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
