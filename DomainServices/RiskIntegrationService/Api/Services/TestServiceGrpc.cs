using DomainServices.RiskIntegrationService.Contracts;
using Microsoft.AspNetCore.Authorization;

namespace DomainServices.RiskIntegrationService.Api.Services;

[Authorize]
public class TestServiceGrpc
    : v1.ITestService
{
    public TestServiceGrpc() { }

    public ValueTask<HalloWorldResponse> HalloWorld(HalloWorldRequest request, CancellationToken cancellationToken = default)
    {
        return ValueTask.FromResult(new HalloWorldResponse { Name = $"My name is {request.Name}" });
    }
}
