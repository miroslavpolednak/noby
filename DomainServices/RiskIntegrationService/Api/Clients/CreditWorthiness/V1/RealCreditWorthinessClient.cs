using DomainServices.RiskIntegrationService.Api.Clients.CreditWorthiness.V1.Contracts;

namespace DomainServices.RiskIntegrationService.Api.Clients.CreditWorthiness.V1;

internal sealed class RealCreditWorthinessClient
    : ICreditWorthinessClient
{
    public async Task<CreditWorthinessCalculation> Calculate(CreditWorthinessCalculationArguments request, CancellationToken cancellationToken)
    {
        return await _httpClient.PostToC4m<CreditWorthinessCalculation>(
            _logger,
            CreditWorthinessStartupExtensions.ServiceName,
            nameof(Calculate),
            _calculateUrl,
            request,
            cancellationToken);
    }

    private readonly HttpClient _httpClient;
    private readonly ILogger<RealCreditWorthinessClient> _logger;

    const string _calculateUrl = "/riskBusinessCase";

    public RealCreditWorthinessClient(HttpClient httpClient, ILogger<RealCreditWorthinessClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }
}
