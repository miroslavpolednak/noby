using DomainServices.RiskIntegrationService.Contracts;

namespace DomainServices.RiskIntegrationService.v1;

[ServiceContract(Name = "DomainServices.RiskIntegrationService.v1.RipService")]
public interface IRipService
{
    ValueTask<CreditWorthinessResponse> CreditWorthiness(CreditWorthinessRequest request, CancellationToken cancellationToken = default);
}
