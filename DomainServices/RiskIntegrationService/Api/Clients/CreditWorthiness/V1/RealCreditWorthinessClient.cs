using DomainServices.RiskIntegrationService.Api.Clients.CreditWorthiness.V1.Contracts;

namespace DomainServices.RiskIntegrationService.Api.Clients.CreditWorthiness.V1;

internal sealed class RealCreditWorthinessClient
    : ICreditWorthinessClient
{
    public async Task<CreditWorthinessCalculation> Calculate(CreditWorthinessCalculationArguments request, CancellationToken cancellationToken)
    {
        var response = await _httpClient
                    .PutAsJsonAsync(_httpClient.BaseAddress + _calculateUrl, request, cancellationToken)
                    .ConfigureAwait(false);

        var result = await response.Content.ReadFromJsonAsync<CreditWorthinessCalculation>(cancellationToken: cancellationToken)
                ?? throw new CisExtServiceResponseDeserializationException(0, CreditWorthinessStartupExtensions.ServiceName, nameof(Calculate), nameof(CreditWorthinessCalculation));

        return result;
    }

    private readonly HttpClient _httpClient;
    private readonly ILogger<RealCreditWorthinessClient> _logger;

    const string _calculateUrl = "/credit-worthiness";

    public RealCreditWorthinessClient(HttpClient httpClient, ILogger<RealCreditWorthinessClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }
}
