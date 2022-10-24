using CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CIS.InternalServices.DocumentDataAggregator.Configuration.Data.EntityConfiguration;

internal class DocumentDataFieldConfiguration : IEntityTypeConfiguration<DocumentDataField>
{
    public void Configure(EntityTypeBuilder<DocumentDataField> builder)
    {
        builder.HasKey(x => new { x.DocumentId, x.DocumentVersion, x.DataFieldId });

        builder.HasOne(x => x.Document);

        builder.HasOne(x => x.DataField);

        builder.Property(x => x.TemplateFieldName).IsRequired();
    }
}