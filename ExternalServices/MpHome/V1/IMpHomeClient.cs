﻿using CIS.Infrastructure.ExternalServicesHelpers;
using ExternalServices.MpHome.V1.Contracts;

namespace ExternalServices.MpHome.V1;

public interface IMpHomeClient
    : IExternalServiceClient
{
    const string Version = "V1";

    /// <summary>
    /// inserts/updates row in table dbo.Uver in KonsDB (according to provided data)
    /// </summary>
    Task UpdateLoan(long loanId, MortgageRequest mortgageRequest, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// inserts/updates row in table dbo.VztahUver in KonsDB (according to provided data)
    /// </summary>
    Task UpdateLoanPartnerLink(long loanId, long partnerId, LoanLinkRequest loanLinkRequest, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// deletes row in table dbo.VztahUver in KonsDB (according to provided data)
    /// </summary>
    Task DeletePartnerLoanLink(long loanId, long partnerId, CancellationToken cancellationToken = default(CancellationToken));

    Task UpdatePartner(long partnerId, PartnerRequest request, CancellationToken cancellationToken = default(CancellationToken));

    Task UpdatePartnerKbId(long partnerId, long kbId, CancellationToken cancellationToken = default);
    
    Task CancelLoan(long loanId, CancellationToken cancellationToken = default);
}
