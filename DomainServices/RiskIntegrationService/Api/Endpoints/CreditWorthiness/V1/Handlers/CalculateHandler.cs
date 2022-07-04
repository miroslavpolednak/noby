namespace DomainServices.RiskIntegrationService.Api.Endpoints.CreditWorthiness.Handlers;

internal sealed class CalculateHandler
    : IRequestHandler<Contracts.CreditWorthiness.CalculateRequest, Contracts.CreditWorthiness.CalculateResponse>
{
    public async Task<Contracts.CreditWorthiness.CalculateResponse> Handle(Contracts.CreditWorthiness.CalculateRequest request, CancellationToken cancellation)
    {
        return null;
    }

    private readonly Clients.CreditWorthiness.V1.ICreditWorthinessClient _client;

    public CalculateHandler(Clients.CreditWorthiness.V1.ICreditWorthinessClient client)
    {
        _client = client;
    }
}
