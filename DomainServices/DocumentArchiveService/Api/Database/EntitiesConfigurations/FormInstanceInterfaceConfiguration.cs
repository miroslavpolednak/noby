using DomainServices.DocumentArchiveService.Api.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomainServices.DocumentArchiveService.Api.Database.EntitiesConfigurations;

public class FormInstanceInterfaceConfiguration : IEntityTypeConfiguration<FormInstanceInterface>
{
    public void Configure(EntityTypeBuilder<FormInstanceInterface> builder)
    {
        builder.HasKey(e => e.DocumentId);

        builder.Property(e => e.DocumentId)
        .HasColumnType("varchar(30)")
        .HasColumnName("DOCUMENT_ID");

        builder.Property(e => e.FormType)
        .HasColumnType("varchar(7)")
        .HasColumnName("FORM_TYPE");

        builder.Property(e => e.Status)
       .HasColumnType("smallint")
       .HasColumnName("STATUS");

        builder.Property(e => e.CreatedAt)
        .HasColumnName("CREATED_AT")
        .HasColumnType("datetime2");

        builder.Property(e => e.FormKind)
        .HasColumnType("char(1)")
        .HasColumnName("FORM_KIND");

        builder.Property(e => e.Cpm)
       .HasColumnType("varchar(10)")
       .HasColumnName("CPM");

        builder.Property(e => e.Icp)
       .HasColumnType("varchar(10)")
       .HasColumnName("ICP");

        builder.Property(e => e.Storno)
       .HasColumnName("STORNO")
       .HasColumnType("tinyint");

        builder.Property(e => e.DataType)
       .HasColumnName("DATA_TYPE")
       .HasColumnType("tinyint");

        builder.Property(e => e.JsonDataClob)
       .HasColumnName("JSON_DATA_CLOB")
       .HasColumnType("varchar(max)");

    }
}
