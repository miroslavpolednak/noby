using CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CIS.InternalServices.DocumentDataAggregator.Configuration.Data.EntityConfiguration;

internal class DataServiceConfiguration : IEntityTypeConfiguration<DataService>
{
    public void Configure(EntityTypeBuilder<DataService> builder)
    {
        builder.HasKey(x => x.DataServiceId);

        builder.Property(x => x.DataServiceName).IsRequired();
    }
}