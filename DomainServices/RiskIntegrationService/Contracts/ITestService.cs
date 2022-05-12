using DomainServices.RiskIntegrationService.Contracts;

namespace DomainServices.RiskIntegrationService.v1;

[ServiceContract(Name = "DomainServices.RiskIntegrationService.v1.TestService")]
public interface ITestService
{
    ValueTask<HalloWorldResponse> HalloWorld(HalloWorldRequest request, CancellationToken cancellationToken = default);
}
