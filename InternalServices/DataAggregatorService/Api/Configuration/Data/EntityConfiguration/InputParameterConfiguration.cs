using CIS.InternalServices.DataAggregatorService.Api.Configuration.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CIS.InternalServices.DataAggregatorService.Api.Configuration.Data.EntityConfiguration;

internal class InputParameterConfiguration : IEntityTypeConfiguration<InputParameter>
{
    public void Configure(EntityTypeBuilder<InputParameter> builder)
    {
        builder.HasKey(x => x.InputParameterId);

        builder.Property(x => x.InputParameterId).ValueGeneratedNever();

        builder.Property(x => x.InputParameterName).HasMaxLength(50).IsRequired();
    }
}