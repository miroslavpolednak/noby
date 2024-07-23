using CIS.Infrastructure.ExternalServicesHelpers;
using ExternalServices.MpHome.V1.Contracts;

namespace ExternalServices.MpHome.V1;

public interface IMpHomeClient
    : IExternalServiceClient
{
    const string Version = "V1";

    Task<List<PartnerResponse>?> SearchPartners(PartnerSearchRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Vraci detail uveru z tabulky dbo.Uver
    /// </summary>
    Task<LoanDetail> GetMortgage(long productId, CancellationToken cancellationToken = default);

	/// <summary>
	/// inserts/updates row in table dbo.Uver in KonsDB (according to provided data)
	/// </summary>
	Task UpdateLoan(long productId, MortgageRequest mortgageRequest, CancellationToken cancellationToken = default);

    /// <summary>
    /// inserts/updates row in table dbo.VztahUver in KonsDB (according to provided data)
    /// </summary>
    Task UpdateLoanPartnerLink(long productId, long partnerId, LoanLinkRequest loanLinkRequest, CancellationToken cancellationToken = default);

    /// <summary>
    /// deletes row in table dbo.VztahUver in KonsDB (according to provided data)
    /// </summary>
    Task DeletePartnerLoanLink(long productId, long partnerId, CancellationToken cancellationToken = default);

    Task UpdatePartner(long partnerId, PartnerRequest request, CancellationToken cancellationToken = default);

    Task UpdatePartnerKbId(long partnerId, long kbId, CancellationToken cancellationToken = default);
    
    Task CancelLoan(long productId, CancellationToken cancellationToken = default);

    Task<bool> CaseExists(long caseId, CancellationToken cancellationToken = default);

    Task<(List<LoanCondition>? Conditions, List<LoanConditionPhase>? Phases)> GetCovenants(long productId, CancellationToken cancellationToken = default);

    Task<bool> PartnerExists(long partnerId, CancellationToken cancellationToken = default);

    Task<List<CaseSearchResponse>?> SearchCases(CaseSearchRequest request, CancellationToken cancellationToken = default);

    Task UpdatePcpId(long productId, string pcpId, CancellationToken cancellationToken = default);

    Task<PartnerResponse?> GetPartner(long partnerId, CancellationToken cancellationToken = default);

    Task<(LoanRetention? Retention, LoanRefixation? Refixation)> GetRefinancing(long productId, CancellationToken cancellationToken = default);
}
