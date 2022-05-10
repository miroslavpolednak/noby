using DomainServices.RiskIntegrationService.Contracts;

namespace DomainServices.RiskIntegrationService.Api.Services;

public partial class TestService
    : Contracts.ITestService
{
    public TestService() { }

    public Task<HalloWorldResponse> HalloWorld(HalloWorldRequest request, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new HalloWorldResponse { Name = "John Doe" });
    }
}
