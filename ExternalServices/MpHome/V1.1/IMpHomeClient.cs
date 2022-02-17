using ExternalServices.MpHome.V1._1.MpHomeWrapper;

namespace ExternalServices.MpHome.V1._1
{
    public interface IMpHomeClient
    {
        /// <summary>
        /// inserts/updates row in table dbo.Uver in KonsDB (according to provided data)
        /// </summary>
        Task<IServiceCallResult> UpdateLoan(long loanId, MortgageRequest mortgageRequest);

        /// <summary>
        /// inserts/updates row in table dbo.VztahUver in KonsDB (according to provided data)
        /// </summary>
        Task<IServiceCallResult> UpdateLoanPartnerLink(long loanId, long partnerId, LoanLinkRequest loanLinkRequest);

        /// <summary>
        /// deletes row in table dbo.VztahUver in KonsDB (according to provided data)
        /// </summary>
        Task<IServiceCallResult> DeletePartnerLoanLink(long loanId, long partnerId);
    }
}
