using CIS.Infrastructure.ExternalServicesHelpers;
using ExternalServices.MpHome.V1.Contracts;

namespace ExternalServices.MpHome.V1;

public interface IMpHomeClient
    : IExternalServiceClient
{
    const string Version = "V1";

    /// <summary>
    /// Vraci detail uveru z tabulky dbo.Uver
    /// </summary>
    Task<LoanDetail?> GetMortgage(long productId, CancellationToken cancellationToken);

	/// <summary>
	/// inserts/updates row in table dbo.Uver in KonsDB (according to provided data)
	/// </summary>
	Task UpdateLoan(long productId, MortgageRequest mortgageRequest, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// inserts/updates row in table dbo.VztahUver in KonsDB (according to provided data)
    /// </summary>
    Task UpdateLoanPartnerLink(long productId, long partnerId, LoanLinkRequest loanLinkRequest, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// deletes row in table dbo.VztahUver in KonsDB (according to provided data)
    /// </summary>
    Task DeletePartnerLoanLink(long productId, long partnerId, CancellationToken cancellationToken = default(CancellationToken));

    Task UpdatePartner(long partnerId, PartnerRequest request, CancellationToken cancellationToken = default(CancellationToken));

    Task UpdatePartnerKbId(long partnerId, long kbId, CancellationToken cancellationToken = default);
    
    Task CancelLoan(long productId, CancellationToken cancellationToken = default);
}
