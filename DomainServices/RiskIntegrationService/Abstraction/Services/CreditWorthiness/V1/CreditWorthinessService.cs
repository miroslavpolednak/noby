using CIS.Infrastructure.Logging;

namespace DomainServices.RiskIntegrationService.Abstraction.Services.CreditWorthiness.V1;

internal class CreditWorthinessService
    : Abstraction.CreditWorthiness.V1.ICreditWorthinessService
{
    public async Task<IServiceCallResult> Calculate(Contracts.CreditWorthiness.V1.CreditWorthinessCalculateRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStarted(nameof(Calculate));
        var result = await _service.Calculate(request, cancellationToken: cancellationToken);
        return new SuccessfulServiceCallResult<Contracts.CreditWorthiness.V1.CreditWorthinessCalculateResponse>(result);
    }

    private readonly ILogger<CreditWorthinessService> _logger;
    private readonly RiskIntegrationService.V1.ICreditWorthinessService _service;
    
    public CreditWorthinessService(RiskIntegrationService.V1.ICreditWorthinessService service, ILogger<CreditWorthinessService> logger)
    {
        _logger = logger;
        _service = service;
    }
}
