using CIS.InternalServices.DataAggregatorService.Api.Configuration.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CIS.InternalServices.DataAggregatorService.Api.Configuration.Data.EntityConfiguration;

internal class DataFieldConfiguration : IEntityTypeConfiguration<DataField>
{
    public void Configure(EntityTypeBuilder<DataField> builder)
    {
        builder.HasKey(x => x.DataFieldId);

        builder.HasOne(x => x.DataService);

        builder.Property(x => x.FieldPath).HasMaxLength(250).IsRequired();

        builder.HasIndex(x => x.FieldPath).IsUnique();

        builder.Property(x => x.DefaultStringFormat).HasMaxLength(50).IsRequired(false);
    }
}