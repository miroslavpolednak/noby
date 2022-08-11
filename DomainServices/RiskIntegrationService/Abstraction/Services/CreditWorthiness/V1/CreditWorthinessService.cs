using CIS.Infrastructure.Logging;

namespace DomainServices.RiskIntegrationService.Abstraction.Services.CreditWorthiness.V2;

internal class CreditWorthinessService
    : Abstraction.CreditWorthiness.V2.ICreditWorthinessService
{
    public async Task<IServiceCallResult> Calculate(Contracts.CreditWorthiness.V2.CreditWorthinessCalculateRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStarted(nameof(Calculate));
        var result = await _service.Calculate(request, cancellationToken: cancellationToken);
        return new SuccessfulServiceCallResult<Contracts.CreditWorthiness.V2.CreditWorthinessCalculateResponse>(result);
    }

    private readonly ILogger<CreditWorthinessService> _logger;
    private readonly RiskIntegrationService.V2.ICreditWorthinessService _service;
    
    public CreditWorthinessService(RiskIntegrationService.V2.ICreditWorthinessService service, ILogger<CreditWorthinessService> logger)
    {
        _logger = logger;
        _service = service;
    }
}
