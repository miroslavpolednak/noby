using DomainServices.RiskIntegrationService.Api.Clients.CustomersExposure.V1.Contracts;

namespace DomainServices.RiskIntegrationService.Api.Clients.CustomersExposure.V1;

internal sealed class RealCustomersExposureClient
    : ICustomersExposureClient
{
    public async Task<LoanApplicationRelatedExposureResult> Calculate(LoanApplicationRelatedExposure request, CancellationToken cancellationToken)
    {
        var response = await _httpClient
                    .PutAsJsonAsync(_httpClient.BaseAddress + _calculateUrl, request, cancellationToken)
                    .ConfigureAwait(false);

        var result = await response.Content.ReadFromJsonAsync<LoanApplicationRelatedExposureResult>(cancellationToken: cancellationToken)
                ?? throw new CisExtServiceResponseDeserializationException(0, CreditWorthinessStartupExtensions.ServiceName, nameof(Calculate), nameof(LoanApplicationRelatedExposureResult));

        return result;
    }

    private readonly HttpClient _httpClient;

    const string _calculateUrl = "/loan-application-exposure-calculation";

    public RealCustomersExposureClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
}
