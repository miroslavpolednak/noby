using CIS.InternalServices.DataAggregatorService.Api.Configuration.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CIS.InternalServices.DataAggregatorService.Api.Configuration.Data.EntityConfiguration;

internal class EasFormDynamicInputParameterConfiguration : IEntityTypeConfiguration<EasFormDynamicInputParameter>
{
    public void Configure(EntityTypeBuilder<EasFormDynamicInputParameter> builder)
    {
        builder.HasKey(x => new { x.EasRequestTypeId, x.EasFormTypeId, x.InputParameterId, x.TargetDataServiceId });

        builder.HasOne(x => x.EasRequestType).WithMany().IsRequired();

        builder.HasOne(x => x.EasFormType).WithMany().IsRequired();

        builder.HasOne(x => x.InputParameter).WithMany().IsRequired();

        builder.HasOne(x => x.TargetDataService).WithMany().IsRequired();

        builder.HasOne(x => x.SourceDataField).WithMany().OnDelete(DeleteBehavior.NoAction).IsRequired();
    }
}