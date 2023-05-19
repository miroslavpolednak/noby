using DomainServices.RiskIntegrationService.ExternalServices.CreditWorthiness.V3.Contracts;

namespace DomainServices.RiskIntegrationService.ExternalServices.CreditWorthiness.V3;

internal sealed class MockCreditWorthinessClient
    : ICreditWorthinessClient
{
    public Task<CreditWorthinessCalculationResponse> Calculate(CreditWorthinessCalculationArguments request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new CreditWorthinessCalculationResponse{});
    }
}
