using _C4M = DomainServices.RiskIntegrationService.Api.Clients.RiskCharakteristics.V1.Contracts;

namespace DomainServices.RiskIntegrationService.Api.Clients.RiskCharakteristics.V1;

internal interface IRiskCharakteristicsClient
{
    Task<_C4M.DTICalculation> CalculateDti(_C4M.DTICalculationArguments request, CancellationToken cancellationToken);

    Task<_C4M.DSTICalculation> CalculateDsti(_C4M.DSTICalculationArguments request, CancellationToken cancellationToken);
}
