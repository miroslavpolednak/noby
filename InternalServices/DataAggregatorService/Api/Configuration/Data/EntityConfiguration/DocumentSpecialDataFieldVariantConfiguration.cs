using CIS.InternalServices.DataAggregatorService.Api.Configuration.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CIS.InternalServices.DataAggregatorService.Api.Configuration.Data.EntityConfiguration;

internal class DocumentSpecialDataFieldVariantConfiguration : IEntityTypeConfiguration<DocumentSpecialDataFieldVariant>
{
    public void Configure(EntityTypeBuilder<DocumentSpecialDataFieldVariant> builder)
    {
        builder.HasKey(v => new { v.DocumentId, v.AcroFieldName, v.DocumentVariant });

        builder.Property(v => v.AcroFieldName).HasMaxLength(100);

        builder.HasOne(v => v.DocumentSpecialDataField);
    }
}