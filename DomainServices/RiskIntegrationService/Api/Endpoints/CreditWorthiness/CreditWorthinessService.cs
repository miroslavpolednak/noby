using DomainServices.RiskIntegrationService.Contracts.CreditWorthiness;
using Microsoft.AspNetCore.Authorization;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.CreditWorthiness;

[Authorize]
public class CreditWorthinessService
    : v1.ICreditWorthinessService
{
    private readonly IMediator _mediator;

    public CreditWorthinessService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async ValueTask<CalculateResponse> Calculate(CalculateRequest request, CancellationToken cancellationToken = default)
        => await _mediator.Send(request, cancellationToken);
}
