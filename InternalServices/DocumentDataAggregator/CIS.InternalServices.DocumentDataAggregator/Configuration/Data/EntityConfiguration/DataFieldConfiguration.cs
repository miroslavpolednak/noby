using CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CIS.InternalServices.DocumentDataAggregator.Configuration.Data.EntityConfiguration;

internal class DataFieldConfiguration : IEntityTypeConfiguration<DataField>
{
    public void Configure(EntityTypeBuilder<DataField> builder)
    {
        builder.HasKey(x => x.DataFieldId);

        builder.HasOne(x => x.DataService);

        builder.Property(x => x.FieldPath).IsRequired();
    }
}