using System.Runtime.Serialization;

namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.RiskBusinessCase
{

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class HouseholdAssessmentDetail
    {
        /// <summary>
        /// severity.
        /// </summary>
        /// <value>severity.</value>
        public long? Id { get; set; }

        /// <summary>
        /// householdId.
        /// </summary>
        /// <value>householdId.</value>
        public long? HouseholdId { get; set; }

        /// <summary>
        /// householdIndex.
        /// </summary>
        /// <value>householdIndex.</value>
        public long? HouseholdIndex { get; set; }

        /// <summary>
        /// assessmentDetail.
        /// </summary>
        /// <value>assessmentDetail.</value>
        public AssessmentDetail AssessmentDetail { get; set; }

        /// <summary>
        /// counterpartyAssessmentDetail.
        /// </summary>
        /// <value>counterpartyAssessmentDetail.</value>
        public CounterpartyAssessmentDetail CounterpartyAssessmentDetail { get; set; }
    }
}
