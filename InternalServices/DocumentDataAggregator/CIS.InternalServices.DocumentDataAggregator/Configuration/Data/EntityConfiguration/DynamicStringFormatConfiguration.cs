using CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CIS.InternalServices.DocumentDataAggregator.Configuration.Data.EntityConfiguration;

internal class DynamicStringFormatConfiguration : IEntityTypeConfiguration<DynamicStringFormat>
{
    public void Configure(EntityTypeBuilder<DynamicStringFormat> builder)
    {
        builder.HasKey(x => x.DynamicStringFormatId);

        builder.Property(x => x.Format).IsRequired();

        builder.Property(x => x.Priority).IsRequired();

        builder.HasMany(x => x.DynamicStringFormatConditions).WithOne().HasForeignKey(x => x.DynamicStringFormatId);
    }
}