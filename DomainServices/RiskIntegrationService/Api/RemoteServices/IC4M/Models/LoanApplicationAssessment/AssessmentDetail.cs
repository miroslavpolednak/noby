using System.Collections.Generic;

namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.LoanApplicationAssessment
{

    /// <summary>
    /// AssessmentDetail
    /// </summary>
    public class AssessmentDetail
    {
        /// <summary>
        /// Assessment detail
        /// </summary>
        public LoanApplicationScore LoanApplicationScore { get; set; }

        /// <summary>
        /// Assessment detail
        /// </summary>
        public LoanApplicationLimit LoanApplicationLimit { get; set; }

        /// <summary>
        /// Assessment detail
        /// </summary>
        public List<RiskCharacteristics> LoanApplicationRiskCharacteristics { get; set; }
    }
}
