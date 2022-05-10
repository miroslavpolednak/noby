using System.Runtime.Serialization;

namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.RiskBusinessCase
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class LoanApplicationCreate
    {
        /// <summary>
        /// identifikátor obchodního případu v C4M.
        /// </summary>
        /// <value>identifikátor obchodního případu v C4M.</value>
        public string RiskBusinessCaseId { get; set; }
    }
}
