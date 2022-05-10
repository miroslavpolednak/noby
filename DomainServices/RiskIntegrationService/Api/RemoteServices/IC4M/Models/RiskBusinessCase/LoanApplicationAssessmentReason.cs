using System.Runtime.Serialization;

namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.RiskBusinessCase
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class LoanApplicationAssessmentReason
    {
        /// <summary>
        /// severity.
        /// </summary>
        /// <value>severity.</value>
        public string Severity { get; set; }

        /// <summary>
        /// code.
        /// </summary>
        /// <value>code.</value>
        public string Code { get; set; }

        /// <summary>
        /// Level
        /// </summary>
        /// <value>Level</value>
        public string Level { get; set; }

        /// <summary>
        /// Weight
        /// </summary>
        /// <value>Weight</value>
        public long? Weight { get; set; }

        /// <summary>
        /// Related Entity Id
        /// </summary>
        /// <value>Related Entity Id</value>
        public AssessmentReasonDetail Detail { get; set; }

    }
}
