using _C4M = DomainServices.RiskIntegrationService.Api.Clients.RiskCharakteristics.V1.Contracts;

namespace DomainServices.RiskIntegrationService.Api.Clients.RiskCharakteristics.V1;

internal sealed class RealRiskCharakteristicsClient
    : IRiskCharakteristicsClient
{
    public async Task<_C4M.DTICalculation> CalculateDti(_C4M.DTICalculationArguments request, CancellationToken cancellationToken)
    {
        var response = await _httpClient
            .PutAsJsonAsync(_httpClient.BaseAddress + "/dti-explicit-calculation", request, HttpClientFactoryExtensions.CustomJsonOptions, cancellationToken)
            .ConfigureAwait(false);

        return await response.Content.ReadFromJsonAsync<_C4M.DTICalculation>(HttpClientFactoryExtensions.CustomJsonOptions, cancellationToken)
                ?? throw new CisExtServiceResponseDeserializationException(0, RiskCharakteristicsStartupExtensions.ServiceName, nameof(CalculateDti), nameof(_C4M.DTICalculation));
    }

    public async Task<_C4M.DSTICalculation> CalculateDsti(_C4M.DSTICalculationArguments request, CancellationToken cancellationToken)
    {
        var response = await _httpClient
            .PutAsJsonAsync(_httpClient.BaseAddress + "/dsti-explicit-calculation", request, HttpClientFactoryExtensions.CustomJsonOptions, cancellationToken)
            .ConfigureAwait(false);

        return await response.Content.ReadFromJsonAsync<_C4M.DSTICalculation>(HttpClientFactoryExtensions.CustomJsonOptions, cancellationToken)
                ?? throw new CisExtServiceResponseDeserializationException(0, RiskCharakteristicsStartupExtensions.ServiceName, nameof(CalculateDsti), nameof(_C4M.DSTICalculation));
    }

    private readonly HttpClient _httpClient;

    public RealRiskCharakteristicsClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
}
