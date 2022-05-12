using System.Runtime.Serialization;

namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.RiskBusinessCase
{

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class LoanApplicationScore
    {
        /// <summary>
        /// value.
        /// </summary>
        /// <value>value.</value>
        public string Value { get; set; }

        /// <summary>
        /// scale.
        /// </summary>
        /// <value>scale.</value>
        public string Scale { get; set; }
    }
}
