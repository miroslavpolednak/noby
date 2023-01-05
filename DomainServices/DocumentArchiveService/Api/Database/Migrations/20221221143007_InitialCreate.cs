using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DomainServices.DocumentArchiveService.Api.Database.Migrations;

/// <inheritdoc />
public partial class InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
                    CREATE SEQUENCE dbo.GenerateDocumentIdSequence  
                	AS bigint
                    START WITH 1  
                    INCREMENT BY 1;  
                """);
        
        migrationBuilder.CreateTable(
            name: "DocumentInterface",
            columns: table => new
            {
                DOCUMENTID = table.Column<string>(name: "DOCUMENT_ID", type: "varchar(30)", nullable: false),
                DOCUMENTDATA = table.Column<byte[]>(name: "DOCUMENT_DATA", type: "varbinary(max)", nullable: false),
                FILENAME = table.Column<string>(type: "nvarchar(64)", nullable: false),
                FILENAMESUFFIX = table.Column<string>(name: "FILENAME_SUFFIX", type: "varchar(10)", nullable: false),
                DESCRIPTION = table.Column<string>(type: "nvarchar(254)", nullable: true),
                CASEID = table.Column<long>(type: "bigint", nullable: false),
                DATUMPRIJETI = table.Column<DateTime>(name: "DATUM_PRIJETI", type: "datetime", nullable: false),
                AUTHORUSERLOGIN = table.Column<string>(name: "AUTHOR_USER_LOGIN", type: "varchar(10)", nullable: false),
                CONTRACTNUMBER = table.Column<string>(name: "CONTRACT_NUMBER", type: "varchar(13)", nullable: true),
                STATUS = table.Column<int>(type: "int", nullable: false, defaultValue: 100),
                STATUSERRORTEXT = table.Column<string>(name: "STATUS_ERROR_TEXT", type: "varchar(1000)", nullable: true),
                FORMID = table.Column<string>(type: "varchar(15)", nullable: true),
                EACODEMAINID = table.Column<int>(name: "EA_CODE_MAIN_ID", type: "int", nullable: false),
                DOCUMENTDIRECTION = table.Column<string>(name: "DOCUMENT_DIRECTION", type: "varchar(1)", nullable: false, defaultValue: "E"),
                FOLDERDOCUMENT = table.Column<string>(name: "FOLDER_DOCUMENT", type: "varchar(1)", nullable: false, defaultValue: "N"),
                FOLDERDOCUMENTID = table.Column<string>(name: "FOLDER_DOCUMENT_ID", type: "varchar(30)", nullable: true),
                KDV = table.Column<byte>(type: "tinyint", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_DocumentInterface", x => x.DOCUMENTID);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "DocumentInterface");
    }
}
