using CIS.InternalServices.DataAggregatorService.Api.Configuration.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CIS.InternalServices.DataAggregatorService.Api.Configuration.Data.EntityConfiguration;

internal class EasFormDataFieldConfiguration : IEntityTypeConfiguration<EasFormDataField>
{
    public void Configure(EntityTypeBuilder<EasFormDataField> builder)
    {
        builder.HasKey(x => x.EasFormDataFieldId);

        builder.Property(x => x.JsonPropertyName).HasMaxLength(250).IsRequired();

        builder.HasIndex(x => new { x.EasRequestTypeId, x.EasFormTypeId, x.JsonPropertyName }).IsUnique();

        builder.HasOne(x => x.EasRequestType).WithMany().IsRequired();

        builder.HasOne(x => x.DataField).WithMany().IsRequired();

        builder.HasOne(x => x.EasFormType).WithMany().IsRequired();
    }
}