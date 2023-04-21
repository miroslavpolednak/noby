using CIS.InternalServices.DataAggregatorService.Api.Configuration.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CIS.InternalServices.DataAggregatorService.Api.Configuration.Data.EntityConfiguration;

internal class DocumentDataFieldVariantConfiguration : IEntityTypeConfiguration<DocumentDataFieldVariant>
{
    public void Configure(EntityTypeBuilder<DocumentDataFieldVariant> builder)
    {
        builder.HasKey(v => new { v.DocumentDataFieldId, v.DocumentVariant });

        builder.Property(v => v.DocumentVariant).HasMaxLength(5);

        builder.HasOne(v => v.DocumentDataField);
    }
}