using System.Runtime.Serialization;

namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.RiskBusinessCase
{

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class LoanSoldProduct
    {
        /// <summary>
        /// Identifikátor obchodního případu.
        /// </summary>
        /// <value>Identifikátor obchodního případu.</value>
        public ResourceIdentifier Id { get; set; }
    }
}
