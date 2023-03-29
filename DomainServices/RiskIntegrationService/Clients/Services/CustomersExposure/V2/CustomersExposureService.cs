using CIS.Infrastructure.Logging;
using DomainServices.RiskIntegrationService.Contracts.CustomersExposure.V2;

namespace DomainServices.RiskIntegrationService.Clients.Services.CustomersExposure.V2;

internal sealed class CustomersExposureService
    : Clients.CustomersExposure.V2.ICustomersExposureServiceClient
{
    public async Task<CustomersExposureCalculateResponse> Calculate(CustomersExposureCalculateRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        return await _service.Calculate(request, cancellationToken: cancellationToken);
    }

    private readonly ICustomersExposureService _service;

    public CustomersExposureService(ICustomersExposureService service)
    {
        _service = service;
    }
}
