using System;
using System.Collections.Generic;

namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.LoanApplicationAssessment
{

    /// <summary>
    /// 
    /// </summary>
    public class LoanApplicationAssessmentExpanded : LoanApplicationAssessment
    {
        /// <summary>
        /// todoSubresource
        /// </summary>
        public Dictionary<string, Object> TodoSubresource { get; set; }
    }
}
