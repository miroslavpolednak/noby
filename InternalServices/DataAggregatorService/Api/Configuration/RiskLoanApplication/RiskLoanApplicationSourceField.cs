namespace CIS.InternalServices.DataAggregatorService.Api.Configuration.RiskLoanApplication;

internal class RiskLoanApplicationSourceField
{
    public DataSource DataSource { get; init; }

    public string FieldPath { get; init; } = null!;

    public string JsonPropertyName { get; init; } = null!;
}