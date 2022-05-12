using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.LoanApplicationExposure
{
    /// <summary>
    /// LoanApplicationRelatedExposur
    /// </summary>
    public class LoanApplicationRelatedExposure 
    {
        /// <summary>
        /// loanApplicationId
        /// </summary>
        public ResourceIdentifier LoanApplicationId { get; set; }

        /// <summary>
        /// riskBusinessCaseId
        /// </summary>
        public ResourceIdentifier RiskBusinessCaseId { get; set; }
    }
}
