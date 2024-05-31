using CIS.Infrastructure.ExternalServicesHelpers;
using ExternalServices.MpHome.V1.Contracts;
using System.Net.Http.Json;
using Microsoft.AspNetCore.WebUtilities;

namespace ExternalServices.MpHome.V1;

internal sealed class RealMpHomeClient(HttpClient _httpClient)
    : IMpHomeClient
{
	public async Task UpdatePcpId(long productId, string pcpId, CancellationToken cancellationToken = default)
	{
		var response = await _httpClient
            .PostAsJsonAsync(_httpClient.BaseAddress + $"/foms/Loan/{productId}/pcpInstId", new LoanPcpInstIdRequest { PcpInstId = pcpId }, cancellationToken)
			.ConfigureAwait(false);
		await response.EnsureSuccessStatusCode(StartupExtensions.ServiceName, cancellationToken);
	}

	public async Task<long?> SearchCases(CaseSearchRequest request, CancellationToken cancellationToken = default)
	{
		var response = await _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + $"/foms/Case/search", request, cancellationToken);
		await response.EnsureSuccessStatusCode(StartupExtensions.ServiceName, cancellationToken);
        if (long.TryParse(await response.SafeReadAsStringAsync(cancellationToken), out long id))
        {
            return id;
        }
        return null;
	}

	public async Task<(List<LoanCondition>? Conditions, List<LoanConditionPhase>? Phases)> GetCovenants(long productId, CancellationToken cancellationToken = default)
	{
		var response = await _httpClient.GetAsync(_httpClient.BaseAddress + $"/foms/Loan/{productId}/conditions", cancellationToken);
		var result = await response.EnsureSuccessStatusAndReadJson<LoanConditionsResponse>(StartupExtensions.ServiceName, cancellationToken);
        return (result.Conditions?.ToList(), result.Phases?.ToList());
	}

	public async Task<(LoanRetention? Retention, LoanRefixation? Refixation)> GetRefinancing(long productId, CancellationToken cancellationToken = default)
	{
		var response = await _httpClient.GetAsync(_httpClient.BaseAddress + $"/foms/Loan/{productId}/retentionRefixation", cancellationToken);
		var result = await response.EnsureSuccessStatusAndReadJson<LoanRetentionResponse>(StartupExtensions.ServiceName, cancellationToken);
		return (result.Retention, result.Refixation);
	}

	public async Task<bool> PartnerExists(long partnerId, CancellationToken cancellationToken = default)
	{
		var response = await _httpClient.GetAsync(_httpClient.BaseAddress + $"/foms/Partner/{partnerId}/exists", cancellationToken);
		return await response.EnsureSuccessStatusAndReadJson<bool>(StartupExtensions.ServiceName, cancellationToken);
	}

	public async Task<bool> CaseExists(long caseId, CancellationToken cancellationToken = default)
	{
		var response = await _httpClient.GetAsync(_httpClient.BaseAddress + $"/foms/Case/{caseId}/exists", cancellationToken);
        await response.EnsureSuccessStatusCode(StartupExtensions.ServiceName, cancellationToken);
        return (await response.SafeReadAsStringAsync(cancellationToken)) == "true";
	}

	public async Task<PartnerResponse?> GetCustomer(long partnerId, CancellationToken cancellationToken = default)
	{
		var response = await _httpClient.GetAsync(_httpClient.BaseAddress + $"/foms/Partner/{partnerId}", cancellationToken);
		return await response.EnsureSuccessStatusAndReadJson<PartnerResponse>(StartupExtensions.ServiceName, cancellationToken);
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