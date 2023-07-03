using DomainServices.RiskIntegrationService.ExternalServices.CreditWorthiness.V3.Contracts;

namespace DomainServices.RiskIntegrationService.ExternalServices.CreditWorthiness.V3;

internal sealed class RealCreditWorthinessClient
    : ICreditWorthinessClient
{
    public async Task<CreditWorthinessCalculationResponse> Calculate(CreditWorthinessCalculationArguments request, CancellationToken cancellationToken)
    {
        var response = await _httpClient
                    .PutAsJsonAsync(_httpClient.BaseAddress + _calculateUrl, request, C4mJsonOptions.CustomJsonOptions, cancellationToken)
                    .ConfigureAwait(false);

        var result = await response.Content.ReadFromJsonAsync<CreditWorthinessCalculationResponse>(C4mJsonOptions.CustomJsonOptions, cancellationToken)
                ?? throw new CisExtServiceResponseDeserializationException(ErrorCodeMapper.ServiceResponseDeserializationException, StartupExtensions.ServiceName, nameof(Calculate), nameof(CreditWorthinessCalculationResponse));

        return result;
    }

    private readonly HttpClient _httpClient;
    
    const string _calculateUrl = "/credit-worthiness";

    public RealCreditWorthinessClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
}
