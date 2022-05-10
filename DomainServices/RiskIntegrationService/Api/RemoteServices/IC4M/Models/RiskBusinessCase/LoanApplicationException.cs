using System.Runtime.Serialization;

namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.RiskBusinessCase
{
    /// <summary>
    /// LoanApplicationException
    /// </summary>
    [DataContract]
    public class LoanApplicationException
    {
        /// <summary>
        /// Arm.
        /// </summary>
        public string Arm { get; set; }

        /// <summary>
        /// ReasonCode.
        /// </summary>
        public string ReasonCode { get; set; }
    }
}
