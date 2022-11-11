using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Migrations
{
    public partial class DocumentDataFieldKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DynamicStringFormat_DocumentDataField_DocumentId_DocumentVersion_DataFieldId",
                table: "DynamicStringFormat");

            migrationBuilder.DropIndex(
                name: "IX_DynamicStringFormat_DocumentId_DocumentVersion_DataFieldId",
                table: "DynamicStringFormat");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DocumentDataField",
                table: "DocumentDataField");

            migrationBuilder.DropColumn(
                name: "DataFieldId",
                table: "DynamicStringFormat");

            migrationBuilder.DropColumn(
                name: "DocumentVersion",
                table: "DynamicStringFormat");

            migrationBuilder.RenameColumn(
                name: "DocumentId",
                table: "DynamicStringFormat",
                newName: "DocumentDataFieldId");

            migrationBuilder.AddColumn<int>(
                name: "DocumentDataFieldId",
                table: "DocumentDataField",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DocumentDataField",
                table: "DocumentDataField",
                column: "DocumentDataFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_DynamicStringFormat_DocumentDataFieldId",
                table: "DynamicStringFormat",
                column: "DocumentDataFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentDataField_DocumentId_DocumentVersion_TemplateFieldName",
                table: "DocumentDataField",
                columns: new[] { "DocumentId", "DocumentVersion", "TemplateFieldName" });

            migrationBuilder.AddForeignKey(
                name: "FK_DynamicStringFormat_DocumentDataField_DocumentDataFieldId",
                table: "DynamicStringFormat",
                column: "DocumentDataFieldId",
                principalTable: "DocumentDataField",
                principalColumn: "DocumentDataFieldId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DynamicStringFormat_DocumentDataField_DocumentDataFieldId",
                table: "DynamicStringFormat");

            migrationBuilder.DropIndex(
                name: "IX_DynamicStringFormat_DocumentDataFieldId",
                table: "DynamicStringFormat");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DocumentDataField",
                table: "DocumentDataField");

            migrationBuilder.DropIndex(
                name: "IX_DocumentDataField_DocumentId_DocumentVersion_TemplateFieldName",
                table: "DocumentDataField");

            migrationBuilder.DropColumn(
                name: "DocumentDataFieldId",
                table: "DocumentDataField");

            migrationBuilder.RenameColumn(
                name: "DocumentDataFieldId",
                table: "DynamicStringFormat",
                newName: "DocumentId");

            migrationBuilder.AddColumn<int>(
                name: "DataFieldId",
                table: "DynamicStringFormat",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DocumentVersion",
                table: "DynamicStringFormat",
                type: "nvarchar(5)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DocumentDataField",
                table: "DocumentDataField",
                columns: new[] { "DocumentId", "DocumentVersion", "DataFieldId" });

            migrationBuilder.CreateIndex(
                name: "IX_DynamicStringFormat_DocumentId_DocumentVersion_DataFieldId",
                table: "DynamicStringFormat",
                columns: new[] { "DocumentId", "DocumentVersion", "DataFieldId" });

            migrationBuilder.AddForeignKey(
                name: "FK_DynamicStringFormat_DocumentDataField_DocumentId_DocumentVersion_DataFieldId",
                table: "DynamicStringFormat",
                columns: new[] { "DocumentId", "DocumentVersion", "DataFieldId" },
                principalTable: "DocumentDataField",
                principalColumns: new[] { "DocumentId", "DocumentVersion", "DataFieldId" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
