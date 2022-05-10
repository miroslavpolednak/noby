using DomainServices.RiskIntegrationService.Contracts;
using Microsoft.AspNetCore.Authorization;

namespace DomainServices.RiskIntegrationService.Api.Endpoints;

[Authorize]
internal class RipServiceGrpc
    : v1.IRipService
{
    private readonly IMediator _mediator;

    public RipServiceGrpc(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async ValueTask<CreditWorthinessResponse> CreditWorthiness(CreditWorthinessRequest request, CancellationToken cancellationToken = default)
        => await _mediator.Send(request, cancellationToken);
}
