using CIS.Infrastructure.Logging;

namespace DomainServices.RiskIntegrationService.Abstraction.Services;

internal class CreditWorthinessService
    : ICreditWorthinessService
{
    public async Task<IServiceCallResult> Calculate(Contracts.CreditWorthiness.CalculateRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStarted(nameof(Calculate));
        var result = await _service.Calculate(request, cancellationToken: cancellationToken);
        return new SuccessfulServiceCallResult<Contracts.CreditWorthiness.CalculateResponse>(result);
    }

    private readonly ILogger<CreditWorthinessService> _logger;
    private readonly v1.ICreditWorthinessService _service;
    
    public CreditWorthinessService(v1.ICreditWorthinessService service, ILogger<CreditWorthinessService> logger)
    {
        _logger = logger;
        _service = service;
    }
}
