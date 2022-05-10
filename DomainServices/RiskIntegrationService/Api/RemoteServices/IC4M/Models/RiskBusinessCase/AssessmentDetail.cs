using System.Collections.Generic;

namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.RiskBusinessCase
{
    /// <summary>
    /// 
    /// </summary>
    public class AssessmentDetail
    {
        /// <summary>
        /// loanApplicationScore.
        /// </summary>
        /// <value>loanApplicationScore.</value>
        public LoanApplicationScore LoanApplicationScore { get; set; }

        /// <summary>
        /// loanApplicationLimit.
        /// </summary>
        /// <value>loanApplicationLimit.</value>
        public LoanApplicationLimit LoanApplicationLimit { get; set; }

        /// <summary>
        /// loanApplicationRiskCharacteristics.
        /// </summary>
        /// <value>loanApplicationRiskCharacteristics.</value>
        public List<RiskCharacteristics> LoanApplicationRiskCharacteristics { get; set; }
    }
}
