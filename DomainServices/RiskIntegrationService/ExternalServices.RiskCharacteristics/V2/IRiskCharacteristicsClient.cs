using _C4M = DomainServices.RiskIntegrationService.ExternalServices.RiskCharacteristics.V2.Contracts;

namespace DomainServices.RiskIntegrationService.ExternalServices.RiskCharacteristics.V2;

public interface IRiskCharacteristicsClient
    : IRiskCharacteristicsClientBase
{
    Task<_C4M.DTICalculation> CalculateDti(_C4M.DTICalculationArguments request, CancellationToken cancellationToken);

    Task<_C4M.DSTICalculation> CalculateDsti(_C4M.DSTICalculationArguments request, CancellationToken cancellationToken);

    const string Version = "V2";
}
