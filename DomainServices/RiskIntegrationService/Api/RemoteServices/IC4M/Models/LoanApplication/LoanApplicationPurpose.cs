namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.LoanApplication
{
    /// <summary>
    /// Parametry domácnosti.
    /// </summary>
    public class LoanApplicationPurpose
    {

        /// <summary>
        /// účel úvěru.
        /// </summary>
        /// <value>účel úvěru.</value>
        public string Code { get; set; }

        /// <summary>
        /// výše požadované výše úvěru pro daný účel.
        /// </summary>
        /// <value>výše požadované výše úvěru pro daný účel.</value>
        public decimal? Amount { get; set; }
    }
}
