using _C4M = DomainServices.RiskIntegrationService.ExternalServices.RiskCharacteristics.V1.Contracts;

namespace DomainServices.RiskIntegrationService.ExternalServices.RiskCharacteristics.V1;

internal sealed class RealRiskCharacteristicsClient
    : IRiskCharacteristicsClient
{
    public async Task<_C4M.DTICalculation> CalculateDti(_C4M.DTICalculationArguments request, CancellationToken cancellationToken)
    {
        var response = await _httpClient
            .PutAsJsonAsync(_httpClient.BaseAddress + "/dti-explicit-calculation", request, C4mJsonOptions.CustomJsonOptions, cancellationToken)
            .ConfigureAwait(false);

        return await response.Content.ReadFromJsonAsync<_C4M.DTICalculation>(C4mJsonOptions.CustomJsonOptions, cancellationToken)
                ?? throw new CisExtServiceResponseDeserializationException(17001, StartupExtensions.ServiceName, nameof(CalculateDti), nameof(_C4M.DTICalculation));
    }

    public async Task<_C4M.DSTICalculation> CalculateDsti(_C4M.DSTICalculationArguments request, CancellationToken cancellationToken)
    {
        var response = await _httpClient
            .PutAsJsonAsync(_httpClient.BaseAddress + "/dsti-explicit-calculation", request, C4mJsonOptions.CustomJsonOptions, cancellationToken)
            .ConfigureAwait(false);

        return await response.Content.ReadFromJsonAsync<_C4M.DSTICalculation>(C4mJsonOptions.CustomJsonOptions, cancellationToken)
                ?? throw new CisExtServiceResponseDeserializationException(17001, StartupExtensions.ServiceName, nameof(CalculateDsti), nameof(_C4M.DSTICalculation));
    }

    private readonly HttpClient _httpClient;

    public RealRiskCharacteristicsClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
}
