using _C4M = DomainServices.RiskIntegrationService.Api.Clients.LoanApplication.V1.Contracts;

namespace DomainServices.RiskIntegrationService.Api.Clients.LoanApplication.V1;

internal sealed class RealLoanApplicationClient
    : ILoanApplicationClient
{
    public async Task<_C4M.LoanApplicationResult> Save(_C4M.LoanApplication request, CancellationToken cancellationToken)
    {
        var response = await _httpClient
            .PostAsJsonAsync(_httpClient.BaseAddress + _calculateUrl, request, HttpClientFactoryExtensions.CustomJsonOptions, cancellationToken)
            .ConfigureAwait(false);

        return await response.Content.ReadFromJsonAsync<_C4M.LoanApplicationResult>(HttpClientFactoryExtensions.CustomJsonOptions, cancellationToken)
                ?? throw new CisExtServiceResponseDeserializationException(0, LoanApplicationStartupExtensions.ServiceName, nameof(Save), nameof(_C4M.LoanApplicationResult));
    }

    private readonly HttpClient _httpClient;
    const string _calculateUrl = "/hf/loan-application";
    
    public RealLoanApplicationClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
}
