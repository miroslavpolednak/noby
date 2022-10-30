using CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CIS.InternalServices.DocumentDataAggregator.Configuration.Data.EntityConfiguration;

internal class DocumentDataFieldConfiguration : IEntityTypeConfiguration<DocumentDataField>
{
    public void Configure(EntityTypeBuilder<DocumentDataField> builder)
    {
        builder.HasKey(x => new { x.DocumentId, x.DocumentVersion, x.DataFieldId });

        builder.Property(x => x.DocumentVersion).HasMaxLength(5);

        builder.Property(x => x.TemplateFieldName).HasMaxLength(100).IsRequired();

        builder.Property(x => x.StringFormat).HasMaxLength(50).IsRequired(false);

        builder.HasOne(x => x.Document);

        builder.HasOne(x => x.DataField);

        builder.HasMany(x => x.DynamicStringFormats).WithOne().HasForeignKey(x => new { x.DocumentId, x.DocumentVersion, x.DataFieldId });
    }
}