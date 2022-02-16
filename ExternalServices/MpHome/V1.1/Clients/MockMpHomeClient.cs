using ExternalServices.MpHome.V1._1.MpHomeWrapper;

namespace ExternalServices.MpHome.V1._1
{
    internal sealed class MockMpHomeClient : IMpHomeClient
    {
       
        public async Task<IServiceCallResult> UpdateLoan(long loanId, MortgageRequest mortgageRequest)
        {
            return await Task.FromResult(new SuccessfulServiceCallResult());
        }

    }
}
