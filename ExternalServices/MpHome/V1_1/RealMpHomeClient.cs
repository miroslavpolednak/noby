using CIS.Infrastructure.ExternalServicesHelpers;
using ExternalServices.MpHome.V1_1.Contracts;
using System.Net.Http.Json;
using Microsoft.AspNetCore.WebUtilities;

namespace ExternalServices.MpHome.V1_1;

internal sealed class RealMpHomeClient 
    : IMpHomeClient
{
    public async Task UpdateLoan(long loanId, MortgageRequest mortgageRequest, CancellationToken cancellationToken = default(CancellationToken))
    {
        var response = await _httpClient
            .PutAsJsonAsync(_httpClient.BaseAddress + $"/foms/Loan/{loanId}", mortgageRequest, cancellationToken)
            .ConfigureAwait(false);
        await response.EnsureSuccessStatusCode(StartupExtensions.ServiceName, cancellationToken);
    }

    public async Task UpdateLoanPartnerLink(long loanId, long partnerId, LoanLinkRequest loanLinkRequest, CancellationToken cancellationToken = default(CancellationToken))
    {
        var response = await _httpClient
            .PutAsJsonAsync(_httpClient.BaseAddress + $"/foms/Loan/{loanId}/link/{partnerId}", loanLinkRequest, cancellationToken)
            .ConfigureAwait(false);
        await response.EnsureSuccessStatusCode(StartupExtensions.ServiceName, cancellationToken);
    }

    public async Task DeletePartnerLoanLink(long loanId, long partnerId, CancellationToken cancellationToken = default(CancellationToken))
    {
        var response = await _httpClient
            .DeleteAsync(_httpClient.BaseAddress + $"/foms/Loan/{loanId}/link/{partnerId}", cancellationToken)
            .ConfigureAwait(false);
        await response.EnsureSuccessStatusCode(StartupExtensions.ServiceName, cancellationToken);
    }

    public async Task UpdatePartner(long partnerId, PartnerRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        var response = await _httpClient
            .PutAsJsonAsync(_httpClient.BaseAddress + $"/foms/Partner/{partnerId}", request, cancellationToken)
            .ConfigureAwait(false);
        await response.EnsureSuccessStatusCode(StartupExtensions.ServiceName, cancellationToken);
    }

    public async Task UpdatePartnerKbId(long partnerId, long kbId, CancellationToken cancellationToken = default)
    {
        var address = QueryHelpers.AddQueryString($"{_httpClient.BaseAddress}/foms/Partner/{partnerId}/kbId", "kbId", kbId.ToString(System.Globalization.CultureInfo.InvariantCulture));

        var response = await _httpClient.PutAsync(address, default, cancellationToken);

        await response.EnsureSuccessStatusCode(StartupExtensions.ServiceName, cancellationToken);
    }

    private readonly HttpClient _httpClient;
    public RealMpHomeClient(HttpClient httpClient)
        => _httpClient = httpClient;
}