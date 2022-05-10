namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.CreditWorthiness
{
    /// <summary>
    /// Dealer
    /// </summary>
    public class Dealer
    {
        /// <summary>
        /// Identifikátor dealera
        /// </summary>
        public ResourceIdentifier Id { get; set; }

        /// <summary>
        /// Identifikátor zprostředkovateské společnosti
        /// </summary>
        public ResourceIdentifier CompanyId { get; set; }
    }
}
