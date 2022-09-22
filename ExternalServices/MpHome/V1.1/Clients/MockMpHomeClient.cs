using ExternalServices.MpHome.V1._1.MpHomeWrapper;

namespace ExternalServices.MpHome.V1._1
{
    internal sealed class MockMpHomeClient : IMpHomeClient
    {
       
        public async Task<IServiceCallResult> UpdateLoan(long loanId, MortgageRequest mortgageRequest)
        {
            return await Task.FromResult(new SuccessfulServiceCallResult());
        }

        public async Task<IServiceCallResult> UpdateLoanPartnerLink(long loanId, long partnerId, LoanLinkRequest loanLinkRequest)
        {
            return await Task.FromResult(new SuccessfulServiceCallResult());
        }

        public async Task<IServiceCallResult> DeletePartnerLoanLink(long loanId, long partnerId)
        {
            return await Task.FromResult(new SuccessfulServiceCallResult());
        }

        public Task<IServiceCallResult> UpdatePartner(long partnerId, PartnerRequest request)
        {
            return Task.FromResult<IServiceCallResult>(new SuccessfulServiceCallResult());
        }
    }
}
