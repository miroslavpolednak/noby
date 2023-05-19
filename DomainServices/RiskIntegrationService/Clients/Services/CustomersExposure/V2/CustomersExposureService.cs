using CIS.Infrastructure.Logging;
using DomainServices.RiskIntegrationService.Contracts.CustomersExposure.V2;

namespace DomainServices.RiskIntegrationService.Clients.Services.CustomersExposure.V2;

internal sealed class CustomersExposureService
    : Clients.CustomersExposure.V2.ICustomersExposureServiceClient
{
    public async Task<CustomerExposureCalculateResponse> Calculate(CustomerExposureCalculateRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        return await _service.Calculate(request, cancellationToken: cancellationToken);
    }

    private readonly ICustomerExposureService _service;

    public CustomersExposureService(ICustomerExposureService service)
    {
        _service = service;
    }
}
