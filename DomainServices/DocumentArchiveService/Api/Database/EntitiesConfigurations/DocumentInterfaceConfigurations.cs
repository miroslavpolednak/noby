using DomainServices.DocumentArchiveService.Api.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomainServices.DocumentArchiveService.Api.Database.EntitiesConfigurations;

public class DocumentInterfaceConfigurations : IEntityTypeConfiguration<DocumentInterface>
{
    public void Configure(EntityTypeBuilder<DocumentInterface> builder)
    {
        builder.HasKey(e => e.DocumentId);

        builder.Property(e => e.DocumentId)
            .HasColumnType("varchar(30)")
            .HasColumnName("DOCUMENT_ID");

        builder.Property(e => e.DocumentData)
            .HasColumnName("DOCUMENT_DATA");

        builder.Property(e => e.FileName)
           .HasColumnName("FILENAME")
           .HasColumnType("nvarchar(64)");

        builder.Property(e => e.FileNameSuffix)
           .HasColumnName("FILENAME_SUFFIX")
           .HasColumnType("varchar(10)");

        builder.Property(e => e.Description)
         .HasColumnName("DESCRIPTION")
         .HasColumnType("nvarchar(254)");

        builder.Property(e => e.CaseId)
         .HasColumnName("CASEID")
         .HasColumnType("bigint");

        builder.Property(e => e.CreatedOn)
        .HasColumnName("CREATED_ON")
        .HasColumnType("datetime");

        builder.Property(e => e.AuthorUserLogin)
         .HasColumnName("AUTHOR_USER_LOGIN")
         .HasColumnType("varchar(10)");

        builder.Property(e => e.ContractNumber)
         .HasColumnName("CONTRACT_NUMBER")
         .HasColumnType("varchar(13)");

        builder.Property(e => e.Status)
         .HasColumnName("STATUS")
         .HasColumnType("int")
         .HasDefaultValue(100);

        builder.Property(e => e.StatusErrorText)
          .HasColumnName("STATUS_ERROR_TEXT")
          .HasColumnType("varchar(1000)");

        builder.Property(e => e.FormId)
          .HasColumnName("FORMID")
          .HasColumnType("varchar(15)");

        builder.Property(e => e.EaCodeMainId)
         .HasColumnName("EA_CODE_MAIN_ID")
         .HasColumnType("int");

        builder.Property(e => e.DocumentDirection)
         .HasColumnName("DOCUMENT_DIRECTION")
         .HasColumnType("varchar(1)")
         .HasDefaultValue("E");

        builder.Property(e => e.FolderDocument)
        .HasColumnName("FOLDER_DOCUMENT")
        .HasColumnType("varchar(1)")
        .HasDefaultValue("N");

        builder.Property(e => e.FolderDocumentId)
        .HasColumnName("FOLDER_DOCUMENT_ID")
        .HasColumnType("varchar(30)");

        builder.Property(e => e.Kdv)
        .HasColumnName("KDV")
        .HasColumnType("tinyint");

        builder.Property(e => e.SendDocumentOnly)
            .HasColumnName("SEND_DOCUMENT_ONLY")
            .HasColumnType("tinyint");
    }
}
