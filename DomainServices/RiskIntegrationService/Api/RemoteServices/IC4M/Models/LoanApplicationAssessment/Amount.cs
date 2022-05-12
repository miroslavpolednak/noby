namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.LoanApplicationAssessment
{

    /// <summary>
    /// Amount
    /// </summary>
    public class Amount
    {
        /// <summary>
        /// Hodnota částky
        /// </summary>
        public decimal? Value { get; set; }

        /// <summary>
        /// Kód měny částky (ISO 4217)
        /// </summary>
        public string CurrencyCode { get; set; }
    }
}
