using _V1 = DomainServices.RiskIntegrationService.Contracts.CustomersExposure.V1;
using Microsoft.AspNetCore.Authorization;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.CustomersExposure.V1;

[Authorize]
public class CustomersExposureService
    : RiskIntegrationService.V1.ICustomersExposureService
{
    private readonly IMediator _mediator;

    public CustomersExposureService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async ValueTask<_V1.CustomersExposureCalculateResponse> Calculate(_V1.CustomersExposureCalculateRequest request, CancellationToken cancellationToken = default)
        => await _mediator.Send(request, cancellationToken);
}
