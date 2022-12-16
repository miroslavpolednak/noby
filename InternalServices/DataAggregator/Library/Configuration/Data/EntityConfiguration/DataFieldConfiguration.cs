using CIS.InternalServices.DataAggregator.Configuration.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CIS.InternalServices.DataAggregator.Configuration.Data.EntityConfiguration;

internal class DataFieldConfiguration : IEntityTypeConfiguration<DataField>
{
    public void Configure(EntityTypeBuilder<DataField> builder)
    {
        builder.HasKey(x => x.DataFieldId);

        builder.HasOne(x => x.DataService);

        builder.Property(x => x.FieldPath).HasMaxLength(500).IsRequired();

        builder.HasIndex(x => x.FieldPath).IsUnique();

        builder.Property(x => x.DefaultStringFormat).HasMaxLength(50).IsRequired(false);
    }
}