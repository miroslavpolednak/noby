namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.LoanApplication
{
    /// <summary>
    /// Parametry domácnosti.
    /// </summary>
    public class ExpensesSummary
    {
        /// <summary>
        /// Kategorie.
        /// </summary>
        /// <value>Kategorie.</value>
        public string Category { get; set; }

        /// <summary>
        /// Množství.
        /// </summary>
        /// <value>Množství.</value>
        public Amount Amount { get; set; }
    }
}
