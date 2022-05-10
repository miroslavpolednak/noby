namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.Shared
{
    /// <summary>
    /// Parametry domácnosti.
    /// </summary>
    public class LoanApplicationDealer
    {
        /// <summary>
        /// Identifikátor dealera.
        /// </summary>
        /// <value>Identifikátor dealera.</value>
        public ResourceIdentifier Id { get; set; }

        /// <summary>
        /// Identifikátor zprostředkovateské společnosti.
        /// </summary>
        /// <value>Identifikátor zprostředkovateské společnosti.</value>
        public ResourceIdentifier CompanyId { get; set; }
    }
}
