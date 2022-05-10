using System;
using System.Collections.Generic;

namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.RiskBusinessCase
{
    /// <summary>
    /// CommitRequest
    /// </summary>
    public class CommitRequest
    {
        /// <summary>
        /// ID dané úvěrové žádosti.
        /// </summary>
        public ResourceIdentifier LoanApplicationId { get; set; }
                
        /// <summary>
        /// IT Channel.
        /// </summary>
        public string ItChannel { get; set; }

        /// <summary>
        /// loanApplicationProduct.
        /// </summary>
        public LoanApplicationProduct LoanApplicationProduct { get; set; }

        /// <summary>
        /// riskBusinessCaseFinalResult.
        /// </summary>
        public string RiskBusinessCaseFinalResult { get; set; }

        /// <summary>
        /// loanSoldProduct.
        /// </summary>
        public LoanSoldProduct LoanSoldProduct { get; set; }

        /// <summary>
        /// approvalLevel.
        /// </summary>
        public string ApprovalLevel { get; set; }

        /// <summary>
        /// Datum schválení.  Format: yyyy-MM-dd 
        /// </summary>
        public DateTime? ApprovalDate { get; set; }

        /// <summary>
        /// loanAgreement.
        /// </summary>
        public LoanAgreement LoanAgreement { get; set; }

        /// <summary>
        /// creator.
        /// </summary>
        public KBGroupPerson Creator { get; set; }

        /// <summary>
        /// approver.
        /// </summary>
        public KBGroupPerson Approver { get; set; }

        /// <summary>
        /// collateralAgreements.
        /// </summary>
        public List<CollateralAgreement> CollateralAgreements { get; set; }
    }
}
