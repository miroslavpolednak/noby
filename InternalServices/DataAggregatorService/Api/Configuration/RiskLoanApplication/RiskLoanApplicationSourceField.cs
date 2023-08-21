namespace CIS.InternalServices.DataAggregatorService.Api.Configuration.RiskLoanApplication;

internal class RiskLoanApplicationSourceField : SourceFieldBase
{
    public string JsonPropertyName { get; init; } = null!;

    public bool UseDefaultInsteadOfNull { get; init; }
}