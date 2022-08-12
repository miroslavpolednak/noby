using _C4M = DomainServices.RiskIntegrationService.Api.Clients.LoanApplication.V1.Contracts;

namespace DomainServices.RiskIntegrationService.Api.Clients.LoanApplication.V1;

internal sealed class RealLoanApplicationClient
    : ILoanApplicationClient
{
    public async Task<_C4M.LoanApplicationResult> Save(_C4M.LoanApplication request, CancellationToken cancellationToken)
    {
        var response = await _httpClient
                    .PutAsJsonAsync(_httpClient.BaseAddress + _calculateUrl, request, cancellationToken)
                    .ConfigureAwait(false);

        var result = await response.Content.ReadFromJsonAsync<_C4M.LoanApplicationResult>(cancellationToken: cancellationToken)
                ?? throw new CisExtServiceResponseDeserializationException(0, CreditWorthinessStartupExtensions.ServiceName, nameof(Save), nameof(_C4M.LoanApplicationResult));

        return result;
    }

    private readonly HttpClient _httpClient;

    const string _calculateUrl = "/hf/loan-application";

    public RealLoanApplicationClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
}
