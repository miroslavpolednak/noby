using _V2 = DomainServices.RiskIntegrationService.Contracts.CustomerExposure.V2;
using Microsoft.AspNetCore.Authorization;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.CustomerExposure.V2;

[Authorize]
public class CustomersExposureService
    : _V2.ICustomerExposureService
{
    private readonly IMediator _mediator;

    public CustomersExposureService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async ValueTask<_V2.CustomerExposureCalculateResponse> Calculate(_V2.CustomerExposureCalculateRequest request, CancellationToken cancellationToken = default)
        => await _mediator.Send(request, cancellationToken);
}
