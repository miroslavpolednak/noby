namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.CreditWorthiness
{
    /// <summary>
    /// HouseholdInstallmentsSummaryHomeCompany.
    /// </summary>
    public class InstallmentsSummaryHomeCompany
    {
        /// <summary>
        /// Product group.
        /// </summary>
        /// <value>Product group.</value>
        public string ProductGroup { get; set; }

        /// <summary>
        /// Množství.
        /// </summary>
        /// <value>Množství.</value>
        public decimal? Amount { get; set; }

        /// <summary>
        /// Množství.
        /// </summary>
        /// <value>Množství.</value>
        public decimal? AmountConsolidated { get; set; }
    }
}
