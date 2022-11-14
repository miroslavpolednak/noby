using CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CIS.InternalServices.DocumentDataAggregator.Configuration.Data.EntityConfiguration;

internal class DynamicStringFormatDataFieldConfiguration : IEntityTypeConfiguration<DynamicStringFormatDataField>
{
    public void Configure(EntityTypeBuilder<DynamicStringFormatDataField> builder)
    {
        builder.HasKey(x => x.DynamicStringFormatDataFieldId);

        builder.Property(x => x.FieldPath).HasMaxLength(500).IsRequired();
    }
}