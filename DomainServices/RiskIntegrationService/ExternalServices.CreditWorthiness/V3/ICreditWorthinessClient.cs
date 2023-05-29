using DomainServices.RiskIntegrationService.ExternalServices.CreditWorthiness.V3.Contracts;

namespace DomainServices.RiskIntegrationService.ExternalServices.CreditWorthiness.V3;

public interface ICreditWorthinessClient
    : ICreditWorthinessClientBase
{
    Task<CreditWorthinessCalculationResponse> Calculate(CreditWorthinessCalculationArguments request, CancellationToken cancellationToken);

    const string Version = "V3";
}
