using DomainServices.RiskIntegrationService.ExternalServices.CustomerExposure.V3.Contracts;

namespace DomainServices.RiskIntegrationService.ExternalServices.CustomerExposure.V3;

internal sealed class RealCustomerExposureClient
    : ICustomerExposureClient
{
    public async Task<LoanApplicationRelatedExposureResult> Calculate(LoanApplicationRelatedExposure request, CancellationToken cancellationToken)
    {
        var response = await _httpClient
            .PutAsJsonAsync(_httpClient.BaseAddress + _calculateUrl, request, C4mJsonOptions.CustomJsonOptions, cancellationToken)
            .ConfigureAwait(false);

        var result = await response.Content.ReadFromJsonAsync<LoanApplicationRelatedExposureResult>(C4mJsonOptions.CustomJsonOptions, cancellationToken)
            ?? throw new CisExtServiceResponseDeserializationException(ErrorCodeMapper.ServiceResponseDeserializationException, StartupExtensions.ServiceName, nameof(Calculate), nameof(LoanApplicationRelatedExposureResult));

        return result;
    }

    private readonly HttpClient _httpClient;
    const string _calculateUrl = "/loan-application-exposure-calculation";

    public RealCustomerExposureClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
}
