using CIS.InternalServices.DataAggregatorService.Api.Configuration.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CIS.InternalServices.DataAggregatorService.Api.Configuration.Data.EntityConfiguration;

internal class RiskLoanApplicationSpecialDataFieldConfiguration : IEntityTypeConfiguration<RiskLoanApplicationSpecialDataField>
{
    public void Configure(EntityTypeBuilder<RiskLoanApplicationSpecialDataField> builder)
    {
        builder.HasKey(x => x.JsonPropertyName);

        builder.Property(x => x.JsonPropertyName).HasMaxLength(250);

        builder.Property(x => x.FieldPath).HasMaxLength(250);

        builder.HasOne(x => x.DataService).WithMany().IsRequired();
    }
}