using _V1 = DomainServices.RiskIntegrationService.Contracts.CreditWorthiness.V1;
using Microsoft.AspNetCore.Authorization;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.CreditWorthiness.V1;

[Authorize]
public class CreditWorthinessService
    : RiskIntegrationService.V1.ICreditWorthinessService
{
    private readonly IMediator _mediator;

    public CreditWorthinessService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async ValueTask<_V1.CreditWorthinessCalculateResponse> Calculate(_V1.CreditWorthinessCalculateRequest request, CancellationToken cancellationToken = default)
        => await _mediator.Send(request, cancellationToken);
}
