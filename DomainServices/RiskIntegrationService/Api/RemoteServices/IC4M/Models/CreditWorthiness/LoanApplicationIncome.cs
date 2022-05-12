namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.CreditWorthiness
{
    /// <summary>
    /// Žádost o půjčku příjem.
    /// </summary>
    public class LoanApplicationIncome
    {
        /// <summary>
        /// Kategorie.
        /// </summary>
        /// <value>Kategorie.</value>
        public string Category { get; set; }

        /// <summary>
        /// Počet měsíců.
        /// </summary>
        /// <value>Počet měsíců.</value>
        public int? Month { get; set; }

        /// <summary>
        /// Množství.
        /// </summary>
        /// <value>Množství.</value>
        public decimal? Amount { get; set; }
    }
}
