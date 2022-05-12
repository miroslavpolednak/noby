using System.Runtime.Serialization;

namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.RiskBusinessCase
{

    /// <summary>
    /// JobPost
    /// </summary>
    [DataContract]
    public class JobPost
    {
        /// <summary>
        /// LoanApplication identity
        /// </summary>
        /// <value>LoanApplication identity</value>
        public long? Id { get; set; }
    }
}
