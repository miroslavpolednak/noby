using CIS.InternalServices.DataAggregator.Configuration.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CIS.InternalServices.DataAggregator.Configuration.Data.EntityConfiguration;

internal class DynamicStringFormatConditionConfiguration : IEntityTypeConfiguration<DynamicStringFormatCondition>
{
    public void Configure(EntityTypeBuilder<DynamicStringFormatCondition> builder)
    {
        builder.HasKey(x => new { x.DynamicStringFormatId, x.DataFieldId });

        builder.Property(x => x.EqualToValue).HasMaxLength(100).IsRequired(false);

        builder.HasOne(x => x.DataField).WithMany();
    }
}