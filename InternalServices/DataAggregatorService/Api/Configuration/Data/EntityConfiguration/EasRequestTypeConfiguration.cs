using CIS.InternalServices.DataAggregatorService.Api.Configuration.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CIS.InternalServices.DataAggregatorService.Api.Configuration.Data.EntityConfiguration;

internal class EasRequestTypeConfiguration : IEntityTypeConfiguration<EasRequestType>
{
    public void Configure(EntityTypeBuilder<EasRequestType> builder)
    {
        builder.HasKey(x => x.EasRequestTypeId);

        builder.Property(x => x.EasRequestTypeId).ValueGeneratedNever();

        builder.Property(x => x.EasRequestTypeName).HasMaxLength(100).IsRequired();
    }
}