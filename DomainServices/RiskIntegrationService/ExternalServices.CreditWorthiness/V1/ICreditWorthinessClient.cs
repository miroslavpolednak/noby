using CIS.Infrastructure.ExternalServicesHelpers;
using DomainServices.RiskIntegrationService.ExternalServices.CreditWorthiness.V1.Contracts;

namespace DomainServices.RiskIntegrationService.ExternalServices.CreditWorthiness.V1;

internal interface ICreditWorthinessClient
    : IExternalServiceClient
{
    Task<CreditWorthinessCalculation> Calculate(CreditWorthinessCalculationArguments request, CancellationToken cancellationToken);

    const string Version = "V1";
}
