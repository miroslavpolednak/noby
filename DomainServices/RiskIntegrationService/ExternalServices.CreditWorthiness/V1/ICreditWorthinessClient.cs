using DomainServices.RiskIntegrationService.ExternalServices.CreditWorthiness.V1.Contracts;

namespace DomainServices.RiskIntegrationService.ExternalServices.CreditWorthiness.V1;

public interface ICreditWorthinessClient
    : ICreditWorthinessClientBase
{
    Task<CreditWorthinessCalculation> Calculate(CreditWorthinessCalculationArguments request, CancellationToken cancellationToken);

    const string Version = "V1";
}
