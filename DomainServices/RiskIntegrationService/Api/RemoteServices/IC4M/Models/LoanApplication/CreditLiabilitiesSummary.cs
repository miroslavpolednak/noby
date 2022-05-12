namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.LoanApplication
{
    /// <summary>
    /// HouseholdCreditLiabilitiesSummaryOutHomeCompany.
    /// </summary>
    public class CreditLiabilitiesSummary
    {
        /// <summary>
        /// Product group (CL,ML,AD,CC, ...)
        /// </summary>
        /// <value>Product group.</value>
        public string ProductClusterCode { get; set; }

        /// <summary>
        /// Množství.
        /// </summary>
        /// <value>Množství.</value>
        public Amount Amount { get; set; }

        /// <summary>
        /// Množství.
        /// </summary>
        /// <value>Množství.</value>
        public Amount AmountConsolidated { get; set; }
    }
}
