using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.LoanApplicationExposure
{
    /// <summary>
    /// ExposureSummaryForApproval
    /// </summary>
    public class ExposureSummaryForApproval 
    {
        /// <summary>
        /// totalExistingExposureKB
        /// </summary>
        public Amount TotalExistingExposureKB { get; set; }

        /// <summary>
        /// totalExistingExposureKBNaturalPerson
        /// </summary>
        public Amount TotalExistingExposureKBNaturalPerson { get; set; }

        /// <summary>
        /// totalExistingExposureKBNonPurpose
        /// </summary>
        public Amount TotalExistingExposureKBNonPurpose { get; set; }

        /// <summary>
        /// totalExistingExposureUnsecured
        /// </summary>
        public Amount TotalExistingExposureUnsecured { get; set; }
    }
}
