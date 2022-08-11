using _V2 = DomainServices.RiskIntegrationService.Contracts.CreditWorthiness.V2;
using Microsoft.AspNetCore.Authorization;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.CreditWorthiness.V2;

[Authorize]
public class CreditWorthinessService
    : RiskIntegrationService.V2.ICreditWorthinessService
{
    private readonly IMediator _mediator;

    public CreditWorthinessService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async ValueTask<_V2.CreditWorthinessCalculateResponse> Calculate(_V2.CreditWorthinessCalculateRequest request, CancellationToken cancellationToken = default)
        => await _mediator.Send(request, cancellationToken);
}
