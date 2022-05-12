namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.LoanApplication
{
    /// <summary>
    /// Částka, Kód měny
    /// </summary>
    public class Amount
    {
        /// <summary>
        /// Hodnota částky
        /// </summary>
        /// <value>Hodnota částky</value>
        public decimal? Value { get; set; }

        /// <summary>
        /// Kód měny částky (ISO 4217)
        /// </summary>
        /// <value>Kód měny částky (ISO 4217)</value>
        public string CurrencyCode { get; set; }
    }
}
