using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Migrations
{
    /// <inheritdoc />
    public partial class DynamicInputParameterPK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EasFormDynamicInputParameter_DataService_TargetDataServiceId",
                table: "EasFormDynamicInputParameter");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EasFormDynamicInputParameter",
                table: "EasFormDynamicInputParameter");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DocumentDynamicInputParameter",
                table: "DocumentDynamicInputParameter");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EasFormDynamicInputParameter",
                table: "EasFormDynamicInputParameter",
                columns: new[] { "EasRequestTypeId", "InputParameterId", "TargetDataServiceId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_DocumentDynamicInputParameter",
                table: "DocumentDynamicInputParameter",
                columns: new[] { "DocumentId", "DocumentVersion", "InputParameterId", "TargetDataServiceId" });

            migrationBuilder.AddForeignKey(
                name: "FK_EasFormDynamicInputParameter_DataService_TargetDataServiceId",
                table: "EasFormDynamicInputParameter",
                column: "TargetDataServiceId",
                principalTable: "DataService",
                principalColumn: "DataServiceId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EasFormDynamicInputParameter_DataService_TargetDataServiceId",
                table: "EasFormDynamicInputParameter");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EasFormDynamicInputParameter",
                table: "EasFormDynamicInputParameter");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DocumentDynamicInputParameter",
                table: "DocumentDynamicInputParameter");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EasFormDynamicInputParameter",
                table: "EasFormDynamicInputParameter",
                columns: new[] { "EasRequestTypeId", "InputParameterId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_DocumentDynamicInputParameter",
                table: "DocumentDynamicInputParameter",
                columns: new[] { "DocumentId", "DocumentVersion", "InputParameterId" });

            migrationBuilder.AddForeignKey(
                name: "FK_EasFormDynamicInputParameter_DataService_TargetDataServiceId",
                table: "EasFormDynamicInputParameter",
                column: "TargetDataServiceId",
                principalTable: "DataService",
                principalColumn: "DataServiceId");
        }
    }
}
