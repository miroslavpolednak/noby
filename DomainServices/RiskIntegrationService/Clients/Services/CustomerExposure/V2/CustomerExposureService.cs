using CIS.Infrastructure.Logging;
using DomainServices.RiskIntegrationService.Contracts.CustomerExposure.V2;

namespace DomainServices.RiskIntegrationService.Clients.Services.CustomersExposure.V2;

internal sealed class CustomerExposureService
    : Clients.CustomerExposure.V2.ICustomerExposureServiceClient
{
    public async Task<CustomerExposureCalculateResponse> Calculate(CustomerExposureCalculateRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        return await _service.Calculate(request, cancellationToken: cancellationToken);
    }

    private readonly ICustomerExposureService _service;

    public CustomerExposureService(ICustomerExposureService service)
    {
        _service = service;
    }
}
