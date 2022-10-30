using CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CIS.InternalServices.DocumentDataAggregator.Configuration.Data.EntityConfiguration;

internal class DynamicStringFormatConditionConfiguration : IEntityTypeConfiguration<DynamicStringFormatCondition>
{
    public void Configure(EntityTypeBuilder<DynamicStringFormatCondition> builder)
    {
        builder.HasKey(x => new { x.DynamicStringFormatId, x.DynamicStringFormatDataFieldId });

        builder.HasOne(x => x.DynamicStringFormatDataField).WithMany();

        builder.Property(x => x.EqualToValue).IsRequired();
    }
}