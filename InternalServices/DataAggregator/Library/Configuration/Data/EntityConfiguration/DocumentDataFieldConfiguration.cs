using CIS.InternalServices.DataAggregator.Configuration.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CIS.InternalServices.DataAggregator.Configuration.Data.EntityConfiguration;

internal class DocumentDataFieldConfiguration : IEntityTypeConfiguration<DocumentDataField>
{
    public void Configure(EntityTypeBuilder<DocumentDataField> builder)
    {
        builder.HasKey(x => x.DocumentDataFieldId);

        builder.HasIndex(x => new { x.DocumentId, x.DocumentVersion, TemplateFieldName = x.AcroFieldName });

        builder.Property(x => x.DocumentId).IsRequired();

        builder.Property(x => x.DataFieldId).IsRequired();

        builder.Property(x => x.DocumentVersion).HasMaxLength(5);

        builder.Property(x => x.AcroFieldName).HasMaxLength(100);

        builder.Property(x => x.StringFormat).HasMaxLength(500).IsRequired(false);

        builder.Property(x => x.DefaultTextIfNull).HasMaxLength(500).IsRequired(false);

        builder.HasOne(x => x.Document);

        builder.HasOne(x => x.DataField);
    }
}