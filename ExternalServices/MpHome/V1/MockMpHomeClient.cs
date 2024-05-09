using ExternalServices.MpHome.V1.Contracts;

namespace ExternalServices.MpHome.V1;

internal sealed class MockMpHomeClient 
    : IMpHomeClient
{
    public Task UpdateLoan(long productId, MortgageRequest mortgageRequest, CancellationToken cancellationToken = default(CancellationToken))
    {
        return Task.CompletedTask;
    }

    public Task UpdateLoanPartnerLink(long productId, long partnerId, LoanLinkRequest loanLinkRequest, CancellationToken cancellationToken = default(CancellationToken))
    {
        return Task.CompletedTask;
    }

    public Task DeletePartnerLoanLink(long productId, long partnerId, CancellationToken cancellationToken = default(CancellationToken))
    {
        return Task.CompletedTask;
    }

    public Task UpdatePartner(long partnerId, PartnerRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        return Task.CompletedTask;
    }

    public Task UpdatePartnerKbId(long partnerId, long kbId, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task CancelLoan(long productId, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
