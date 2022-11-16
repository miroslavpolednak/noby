using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Migrations
{
    /// <inheritdoc />
    public partial class EasFormDynamicParameter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    table.PrimaryKey("PK_EasFormDynamicInputParameter", x => new { x.EasRequestTypeId, x.InputParameterId });
                    table.ForeignKey(
                        name: "FK_EasFormDynamicInputParameter_DataField_SourceDataFieldId",
                        column: x => x.SourceDataFieldId,
                        principalTable: "DataField",
                        principalColumn: "DataFieldId");
                    table.ForeignKey(
                        name: "FK_EasFormDynamicInputParameter_DataService_TargetDataServiceId",
                        column: x => x.TargetDataServiceId,
                        principalTable: "DataService",
                        principalColumn: "DataServiceId");
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EasFormDynamicInputParameter");
        }
    }
}
