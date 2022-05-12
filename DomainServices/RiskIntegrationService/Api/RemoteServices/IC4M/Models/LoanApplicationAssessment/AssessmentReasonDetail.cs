using System.Collections.Generic;

namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.LoanApplicationAssessment
{

    /// <summary>
    /// AssessmentReasonDetail
    /// </summary>
    public class AssessmentReasonDetail
    {
        /// <summary>
        /// Target
        /// </summary>
        public string Target { get; set; }

        /// <summary>
        /// Desc
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// Result
        /// </summary>
        public string Result { get; set; }
        
        /// <summary>
        /// Target
        /// </summary>
        public List<Resource> Resource { get; set; }
    }
}
