using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.RiskBusinessCase
{

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class LoanApplicationAssessment
    {
        /// <summary>
        /// Jednoznačný identifikátor žádosti o založení Business Case.
        /// </summary>
        /// <value>Jednoznačný identifikátor žádosti o založení Business Case.</value>
        public ResourceIdentifier Id { get; set; }

        /// <summary>
        /// Jednoznačný identifikátor žádosti o založení Business Case.
        /// </summary>
        /// <value>Jednoznačný identifikátor žádosti o založení Business Case.</value>
        public ResourceIdentifier LoanApplicationId { get; set; }

        /// <summary>
        /// identifikátor obchodního případu v C4M.
        /// </summary>
        /// <value>identifikátor obchodního případu v C4M.</value>
        public ResourceIdentifier RiskBusinessCaseId { get; set; }

        /// <summary>
        /// assessmentResult.
        /// </summary>
        /// <value>assessmentResult.</value>
        public long? AssessmentResult { get; set; }

        /// <summary>
        /// loanApplicationAssessmentReason.
        /// </summary>
        /// <value>loanApplicationAssessmentReason.</value>
        public List<LoanApplicationAssessmentReason> LoanApplicationAssessmentReason { get; set; }

        /// <summary>
        /// assessmentDetail.
        /// </summary>
        /// <value>assessmentDetail.</value>
        public AssessmentDetail AssessmentDetail { get; set; }

        /// <summary>
        /// householdAssessmentDetail.
        /// </summary>
        /// <value>householdAssessmentDetail.</value>
        public List<HouseholdAssessmentDetail> HouseholdAssessmentDetail { get; set; }

        /// <summary>
        /// counterpartyAssessmentDetail.
        /// </summary>
        /// <value>counterpartyAssessmentDetail.</value>
        public List<CounterpartyAssessmentDetail> CounterpartyAssessmentDetail { get; set; }
    }
}
