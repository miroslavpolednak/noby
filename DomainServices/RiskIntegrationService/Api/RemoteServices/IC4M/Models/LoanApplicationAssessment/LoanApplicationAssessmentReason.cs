namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.LoanApplicationAssessment
{

    /// <summary>
    /// LoanApplicationAssessmentReason
    /// </summary>
    public class LoanApplicationAssessmentReason
    {
        /// <summary>
        /// Code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Level
        /// </summary>
        public string Level { get; set; }

        /// <summary>
        /// Weight
        /// </summary>
        public long? Weight { get; set; }

        /// <summary>
        /// Related Entity Id
        /// </summary>
        public AssessmentReasonDetail Detail { get; set; }
    }
}
