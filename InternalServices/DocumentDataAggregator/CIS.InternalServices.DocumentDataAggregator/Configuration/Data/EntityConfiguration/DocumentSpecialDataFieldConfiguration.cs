using CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CIS.InternalServices.DocumentDataAggregator.Configuration.Data.EntityConfiguration;

internal class DocumentSpecialDataFieldConfiguration : IEntityTypeConfiguration<DocumentSpecialDataField>
{
    public void Configure(EntityTypeBuilder<DocumentSpecialDataField> builder)
    {
        builder.HasKey(x => new { x.DocumentId, x.FieldPath });

        builder.HasOne(x => x.Document);

        builder.HasOne(x => x.DataService).WithMany().IsRequired();

        builder.Property(x => x.TemplateFieldName).IsRequired();
    }
}