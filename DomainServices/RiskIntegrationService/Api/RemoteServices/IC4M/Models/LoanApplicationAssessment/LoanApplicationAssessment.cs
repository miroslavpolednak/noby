using System;
using System.Collections.Generic;

namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.LoanApplicationAssessment
{

    /// <summary>
    /// LoanApplicationAssessment
    /// </summary>
    public class LoanApplicationAssessment
    {
        /// <summary>
        /// ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// LoanApplication resource-identifier
        /// </summary>
        public ResourceIdentifier LoanApplicationId { get; set; }

        /// <summary>
        /// Risk BusinessCase resource-identifier
        /// </summary>
        public ResourceIdentifier RiskBusinesscaseId { get; set; }

        /// <summary>
        /// Gets or Sets RiskBusinesscaseExpirationDate
        /// </summary>
        public DateTime? RiskBusinesscaseExpirationDate { get; set; }

        /// <summary>
        /// Assessment result
        /// </summary>
        public long? AssessmentResult { get; set; }

        /// <summary>
        /// Gets or Sets StandardRiskCosts
        /// </summary>
        public float? StandardRiskCosts { get; set; }

        /// <summary>
        /// Gets or Sets GlTableCode
        /// </summary>
        public long? GlTableCode { get; set; }

        /// <summary>
        /// LoanApplicationAssessment resource-identifier
        /// </summary>
        public List<LoanApplicationAssessmentReason> LoanApplicationAssessmentReason { get; set; }

        /// <summary>
        /// Assessment detail
        /// </summary>
        public AssessmentDetail AssessmentDetail { get; set; }

        /// <summary>
        /// HouseholdAssessmentDetail
        /// </summary>
        public List<HouseholdAssessmentDetail> HouseholdAssessmentDetail { get; set; }

        /// <summary>
        /// HouseholdAssessmentDetail
        /// </summary>
        public List<CounterpartyAssessmentDetail> CounterpartyAssessmentDetail { get; set; }

        /// <summary>
        /// CollateralRiskCharacteristics
        /// </summary>
        public List<CollateralRiskCharacteristics> CollateralRiskCharacteristics { get; set; }

        /// <summary>
        /// LoanApplicationAssessment version
        /// </summary>
        public SemanticVersion Version { get; set; }

        /// <summary>
        /// created
        /// </summary>
        public Change Created { get; set; }

        /// <summary>
        /// updated
        /// </summary>
        public Change Updated { get; set; }

        /// <summary>
        /// TODO ...
        /// </summary>
        public string SomeField { get; set; }

        /// <summary>
        /// Gets or Sets ResourceIdentifier
        /// </summary>
        public ResourceIdentifier ResourceIdentifier { get; set; }
    }
}
