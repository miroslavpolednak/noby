using DomainServices.RiskIntegrationService.Api.Clients.CreditWorthiness.V1.Contracts;

namespace DomainServices.RiskIntegrationService.Api.Clients.CreditWorthiness.V1;

internal interface ICreditWorthinessClient
{
    Task<CreditWorthinessCalculation> Calculate(CreditWorthinessCalculationArguments request, CancellationToken cancellationToken);
}
