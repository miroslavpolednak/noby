using CIS.Infrastructure.Logging;
using DomainServices.RiskIntegrationService.Contracts.CustomersExposure.V2;

namespace DomainServices.RiskIntegrationService.Clients.Services.CustomersExposure.V2;

internal class CustomersExposureService
    : Clients.CustomersExposure.V2.ICustomersExposureServiceAbstraction
{
    public async Task<IServiceCallResult> Calculate(CustomersExposureCalculateRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStarted(nameof(Calculate));
        var result = await _service.Calculate(request, cancellationToken: cancellationToken);
        return new SuccessfulServiceCallResult<CustomersExposureCalculateResponse>(result);
    }

    private readonly ILogger<CustomersExposureService> _logger;
    private readonly ICustomersExposureService _service;

    public CustomersExposureService(ICustomersExposureService service, ILogger<CustomersExposureService> logger)
    {
        _logger = logger;
        _service = service;
    }
}
