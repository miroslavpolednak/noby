using CIS.Core.Results;
using CIS.Infrastructure.Logging;
using Microsoft.Extensions.Logging;

namespace DomainServices.RiskIntegrationService.Abstraction.Services;

internal class RipService : IRipServiceAbstraction
{
    public async Task<IServiceCallResult> CreditWorthiness(Contracts.CreditWorthinessRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStarted(nameof(RiskIntegrationService));
        var result = await _service.CreditWorthiness(request, cancellationToken: cancellationToken);
        return new SuccessfulServiceCallResult<Contracts.CreditWorthinessResponse>(result);
    }

    private readonly ILogger<RipService> _logger;
    private readonly DomainServices.RiskIntegrationService.v1.IRipService _service;

    public RipService(
        ILogger<RipService> logger,
        DomainServices.RiskIntegrationService.v1.IRipService service)
    {
        _service = service;
        _logger = logger;
    }
}
