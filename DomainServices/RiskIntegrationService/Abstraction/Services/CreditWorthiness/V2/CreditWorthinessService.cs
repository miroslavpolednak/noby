using CIS.Infrastructure.Logging;
using DomainServices.RiskIntegrationService.Contracts.CreditWorthiness.V2;

namespace DomainServices.RiskIntegrationService.Abstraction.Services.CreditWorthiness.V2;

internal class CreditWorthinessService
    : Abstraction.CreditWorthiness.V2.ICreditWorthinessService
{
    public async Task<IServiceCallResult> Calculate(CreditWorthinessCalculateRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStarted(nameof(Calculate));
        var result = await _service.Calculate(request, cancellationToken: cancellationToken);
        return new SuccessfulServiceCallResult<CreditWorthinessCalculateResponse>(result);
    }

    private readonly ILogger<CreditWorthinessService> _logger;
    private readonly ICreditWorthinessService _service;
    
    public CreditWorthinessService(ICreditWorthinessService service, ILogger<CreditWorthinessService> logger)
    {
        _logger = logger;
        _service = service;
    }
}
