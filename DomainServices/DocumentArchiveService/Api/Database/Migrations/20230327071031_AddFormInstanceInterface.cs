using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DomainServices.DocumentArchiveService.Api.Database.Migrations;

/// <inheritdoc />
public partial class AddFormInstanceInterface : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "FormInstanceInterface",
            columns: table => new
            {
                DOCUMENT_ID = table.Column<string>(type: "varchar(30)", nullable: false),
                FORM_TYPE = table.Column<string>(type: "varchar(7)", nullable: true),
                STATUS = table.Column<short>(type: "smallint", nullable: true),
                FORM_KIND = table.Column<string>(type: "char(1)", nullable: true),
                CPM = table.Column<string>(type: "varchar(10)", nullable: true),
                ICP = table.Column<string>(type: "varchar(10)", nullable: true),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                STORNO = table.Column<byte>(type: "tinyint", nullable: true),
                DATA_TYPE = table.Column<byte>(type: "tinyint", nullable: true),
                JSON_DATA_CLOB = table.Column<string>(type: "varchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_FormInstanceInterface", x => x.DOCUMENT_ID);
                table.ForeignKey(
                    name: "FK_FormInstanceInterface_DocumentInterface_DOCUMENT_ID",
                    column: x => x.DOCUMENT_ID,
                    principalTable: "DocumentInterface",
                    principalColumn: "DOCUMENT_ID",
                    onDelete: ReferentialAction.Cascade);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "FormInstanceInterface");
    }
}
