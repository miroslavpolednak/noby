using _C4M = DomainServices.RiskIntegrationService.ExternalServices.RiskCharacteristics.V1.Contracts;

namespace DomainServices.RiskIntegrationService.ExternalServices.RiskCharacteristics.V1;

internal sealed class MockRiskCharacteristicsClient
    : IRiskCharacteristicsClient
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
