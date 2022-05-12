using DomainServices.RiskIntegrationService.Contracts;
using Microsoft.AspNetCore.Authorization;

namespace DomainServices.RiskIntegrationService.Api.Endpoints;

[Authorize]
internal class TestServiceGrpc
    : v1.ITestService
{
    private readonly IMediator _mediator;

    public TestServiceGrpc(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async ValueTask<HalloWorldResponse> HalloWorld(HalloWorldRequest request, CancellationToken cancellationToken = default)
        => await _mediator.Send(request, cancellationToken);
}
