using CIS.InternalServices.DataAggregatorService.Api.Configuration.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CIS.InternalServices.DataAggregatorService.Api.Configuration.Data.EntityConfiguration;

internal class DataServiceConfiguration : IEntityTypeConfiguration<DataService>
{
    public void Configure(EntityTypeBuilder<DataService> builder)
    {
        builder.HasKey(x => x.DataServiceId);

        builder.Property(x => x.DataServiceId).ValueGeneratedNever();

        builder.Property(x => x.DataServiceName).HasMaxLength(50).IsRequired();
    }
}