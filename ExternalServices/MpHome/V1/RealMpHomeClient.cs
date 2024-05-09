using CIS.Infrastructure.ExternalServicesHelpers;
using ExternalServices.MpHome.V1.Contracts;
using System.Net.Http.Json;
using Microsoft.AspNetCore.WebUtilities;

namespace ExternalServices.MpHome.V1;

internal sealed class RealMpHomeClient(HttpClient _httpClient)
    : IMpHomeClient
{
	public async Task<long?> SearchCases(CaseSearchRequest request, CancellationToken cancellationToken = default)
	{
		var response = await _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + $"/foms/Case/search", request, cancellationToken);
		return await response.EnsureSuccessStatusAndReadJson<long>(StartupExtensions.ServiceName, cancellationToken);
	}

	public async Task<List<LoanCondition>> GetCovenants(long productId, CancellationToken cancellationToken = default)
	{
		var response = await _httpClient.GetAsync(_httpClient.BaseAddress + $"/foms/Loan/{productId}/conditions", cancellationToken);
		return await response.EnsureSuccessStatusAndReadJson<List<LoanCondition>>(StartupExtensions.ServiceName, cancellationToken);
	}

	public async Task<bool> PartnerExists(long partnerId, CancellationToken cancellationToken = default)
	{
		var response = await _httpClient.GetAsync(_httpClient.BaseAddress + $"/foms/Partner/{partnerId}/exists", cancellationToken);
		return await response.EnsureSuccessStatusAndReadJson<bool>(StartupExtensions.ServiceName, cancellationToken);
	}

	public async Task<bool> CaseExists(long caseId, CancellationToken cancellationToken = default)
	{
		var response = await _httpClient.GetAsync(_httpClient.BaseAddress + $"/foms/Case/{caseId}/exists", cancellationToken);
		return await response.EnsureSuccessStatusAndReadJson<bool>(StartupExtensions.ServiceName, cancellationToken);
	}

	public async Task<LoanDetail?> GetMortgage(long productId, CancellationToken cancellationToken = default)
    {
		var response =  await _httpClient.GetAsync(_httpClient.BaseAddress + $"/foms/Loan/{productId}", cancellationToken);
		return await response.EnsureSuccessStatusAndReadJson<Contracts.LoanDetail>(StartupExtensions.ServiceName, cancellationToken);
	}

    public async Task UpdateLoan(long productId, MortgageRequest mortgageRequest, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .PutAsJsonAsync(_httpClient.BaseAddress + $"/foms/Loan/{productId}", mortgageRequest, cancellationToken)
            .ConfigureAwait(false);
        await response.EnsureSuccessStatusCode(StartupExtensions.ServiceName, cancellationToken);
    }

    public async Task CancelLoan(long productId, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.DeleteAsync($"{_httpClient.BaseAddress}/foms/Loan/{productId}", cancellationToken);

        await response.EnsureSuccessStatusCode(StartupExtensions.ServiceName, cancellationToken);
    }

    public async Task UpdateLoanPartnerLink(long productId, long partnerId, LoanLinkRequest loanLinkRequest, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .PutAsJsonAsync(_httpClient.BaseAddress + $"/foms/Loan/{productId}/link/{partnerId}", loanLinkRequest, cancellationToken)
            .ConfigureAwait(false);
        await response.EnsureSuccessStatusCode(StartupExtensions.ServiceName, cancellationToken);
    }

    public async Task DeletePartnerLoanLink(long productId, long partnerId, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .DeleteAsync(_httpClient.BaseAddress + $"/foms/Loan/{productId}/link/{partnerId}", cancellationToken)
            .ConfigureAwait(false);
        await response.EnsureSuccessStatusCode(StartupExtensions.ServiceName, cancellationToken);
    }

    public async Task UpdatePartner(long partnerId, PartnerRequest request, CancellationToken cancellationToken = default)
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
}