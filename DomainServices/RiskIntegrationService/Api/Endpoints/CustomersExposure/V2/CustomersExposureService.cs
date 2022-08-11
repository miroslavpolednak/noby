using _V2 = DomainServices.RiskIntegrationService.Contracts.CustomersExposure.V2;
using Microsoft.AspNetCore.Authorization;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.CustomersExposure.V2;

[Authorize]
public class CustomersExposureService
    : RiskIntegrationService.V2.ICustomersExposureService
{
    private readonly IMediator _mediator;

    public CustomersExposureService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async ValueTask<_V2.CustomersExposureCalculateResponse> Calculate(_V2.CustomersExposureCalculateRequest request, CancellationToken cancellationToken = default)
        => await _mediator.Send(request, cancellationToken);
}
