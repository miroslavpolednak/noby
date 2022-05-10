using System.Runtime.Serialization;

namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.RiskBusinessCase
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class LoanAgreement
    {
        /// <summary>
        /// Identifikátor obchodního případu.
        /// </summary>
        /// <value>Identifikátor obchodního případu.</value>
        public string DistributionChannel { get; set; }

        /// <summary>
        /// Identifikátor obchodního případu.
        /// </summary>
        /// <value>Identifikátor obchodního případu.</value>
        public string SignatureType { get; set; }
    }
}
