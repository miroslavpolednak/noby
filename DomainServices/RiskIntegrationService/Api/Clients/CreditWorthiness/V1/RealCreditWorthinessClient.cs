using DomainServices.RiskIntegrationService.Api.Clients.CreditWorthiness.V1.Contracts;

namespace DomainServices.RiskIntegrationService.Api.Clients.CreditWorthiness.V1;

internal sealed class RealCreditWorthinessClient
    : ICreditWorthinessClient
{
    public async Task<CreditWorthinessCalculation> Calculate(CreditWorthinessCalculationArguments request, CancellationToken cancellationToken)
    {
        var response = await _httpClient
                    .PutAsJsonAsync(_httpClient.BaseAddress + _calculateUrl, request, HttpClientFactoryExtensions.CustomJsonOptions, cancellationToken)
                    .ConfigureAwait(false);

        var result = await response.Content.ReadFromJsonAsync<CreditWorthinessCalculation>(HttpClientFactoryExtensions.CustomJsonOptions, cancellationToken)
                ?? throw new CisExtServiceResponseDeserializationException(0, CreditWorthinessStartupExtensions.ServiceName, nameof(Calculate), nameof(CreditWorthinessCalculation));

        return result;
    }

    private readonly HttpClient _httpClient;
    
    const string _calculateUrl = "/credit-worthiness";

    public RealCreditWorthinessClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
}
