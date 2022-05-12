using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.LoanApplicationExposure
{
    /// <summary>
    /// LoanApplicationRelatedExposureResult
    /// </summary>
    public class LoanApplicationRelatedExposureResult 
    {
        /// <summary>
        /// loanApplicationCounterparty
        /// </summary>
        public List<LoanApplicationCounterparty> LoanApplicationCounterparty { get; set; }

        /// <summary>
        /// exposureSummary
        /// </summary>
        public List<ExposureSummaryForApproval> ExposureSummary { get; set; }
    }
}
