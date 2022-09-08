using _C4M = DomainServices.RiskIntegrationService.Api.Clients.RiskCharakteristics.V1.Contracts;

namespace DomainServices.RiskIntegrationService.Api.Clients.RiskCharakteristics.V1;

internal sealed class MockRiskCharakteristicsClient
    : IRiskCharakteristicsClient
{
    public Task<_C4M.DTICalculation> CalculateDti(_C4M.DTICalculationArguments request, CancellationToken cancellationToken)
    {
        return Task.FromResult<_C4M.DTICalculation>(new _C4M.DTICalculation
        {
            Dti = 1
        });
    }

    public Task<_C4M.DSTICalculation> CalculateDsti(_C4M.DSTICalculationArguments request, CancellationToken cancellationToken)
    {
        return Task.FromResult<_C4M.DSTICalculation>(new _C4M.DSTICalculation
        {
            Dsti = 1
        });
    }
}
