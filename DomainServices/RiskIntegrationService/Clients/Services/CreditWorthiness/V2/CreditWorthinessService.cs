using DomainServices.RiskIntegrationService.Contracts.CreditWorthiness.V2;

namespace DomainServices.RiskIntegrationService.Clients.Services.CreditWorthiness.V2;

internal sealed class CreditWorthinessService
    : Clients.CreditWorthiness.V2.ICreditWorthinessServiceClient
{
    public async Task<CreditWorthinessCalculateResponse> Calculate(CreditWorthinessCalculateRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        return await _service.Calculate(request, cancellationToken: cancellationToken);
    }

    public async Task<CreditWorthinessSimpleCalculateResponse> SimpleCalculate(CreditWorthinessSimpleCalculateRequest request, CancellationToken cancellationToken = default)
    {
        return await _service.SimpleCalculate(request, cancellationToken: cancellationToken);
    }

    private readonly ICreditWorthinessService _service;
    
    public CreditWorthinessService(ICreditWorthinessService service)
    {
        _service = service;
    }
}
