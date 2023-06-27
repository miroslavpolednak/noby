using CIS.InternalServices.DataAggregatorService.Api.Configuration.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CIS.InternalServices.DataAggregatorService.Api.Configuration.Data.EntityConfiguration;

internal class RiskLoanApplicationDataFieldConfiguration : IEntityTypeConfiguration<RiskLoanApplicationDataField>
{
    public void Configure(EntityTypeBuilder<RiskLoanApplicationDataField> builder)
    {
        builder.HasKey(x => new { x.DataFieldId, x.JsonPropertyName });

        builder.Property(x => x.JsonPropertyName).HasMaxLength(250);

        builder.HasOne(x => x.DataField);
    }
}