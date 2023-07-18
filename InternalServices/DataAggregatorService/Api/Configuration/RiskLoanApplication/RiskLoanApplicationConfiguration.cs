namespace CIS.InternalServices.DataAggregatorService.Api.Configuration.RiskLoanApplication;

internal class RiskLoanApplicationConfiguration
{
    public required InputConfig InputConfig { get; init; }

    public required IReadOnlyCollection<RiskLoanApplicationSourceField> SourceFields { get; init; }
}