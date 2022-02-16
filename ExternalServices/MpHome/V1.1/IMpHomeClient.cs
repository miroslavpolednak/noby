using ExternalServices.MpHome.V1._1.MpHomeWrapper;

namespace ExternalServices.MpHome.V1._1
{
    public interface IMpHomeClient
    {
        /// <summary>
        /// inserts/updates row in table dbo.uver in KonsDB (according to provided data)
        /// </summary>
        Task<IServiceCallResult> UpdateLoan(long loanId, MortgageRequest mortgageRequest);
    }
}
