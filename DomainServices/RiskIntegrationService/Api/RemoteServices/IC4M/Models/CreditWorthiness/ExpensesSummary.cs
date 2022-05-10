namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.CreditWorthiness
{
    /// <summary>
    /// Parametry domácnosti.
    /// </summary>
    public class ExpensesSummary
    {
        /// <summary>
        /// Kategorie.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Množství.
        /// </summary>
        public decimal? Amount { get; set; }
    }
}
