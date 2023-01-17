using CIS.InternalServices.DataAggregatorService.Api.Configuration.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CIS.InternalServices.DataAggregatorService.Api.Configuration.Data.EntityConfiguration;

internal class EasFormSpecialDataFieldConfiguration : IEntityTypeConfiguration<EasFormSpecialDataField>
{
    public void Configure(EntityTypeBuilder<EasFormSpecialDataField> builder)
    {
        builder.HasKey(x => new { x.EasRequestTypeId, x.JsonPropertyName, x.EasFormTypeId });

        builder.Property(x => x.JsonPropertyName).HasMaxLength(500);

        builder.Property(x => x.FieldPath).HasMaxLength(500).IsRequired();

        builder.HasOne(x => x.EasRequestType).WithMany().IsRequired();

        builder.HasOne(x => x.EasFormType).WithMany().IsRequired();

        builder.HasOne(x => x.DataService).WithMany().IsRequired();
    }
}