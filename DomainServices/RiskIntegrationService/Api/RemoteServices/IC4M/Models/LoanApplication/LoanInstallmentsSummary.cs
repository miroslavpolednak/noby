namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.LoanApplication
{
    /// <summary>
    /// householdInstallmentsSummaryOutHomeCompany
    /// </summary>
    public class LoanInstallmentsSummary
    {
        /// <summary>
        /// Product group.
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
