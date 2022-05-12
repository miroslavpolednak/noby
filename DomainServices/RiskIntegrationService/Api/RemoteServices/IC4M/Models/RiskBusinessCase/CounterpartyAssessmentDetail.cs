
namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.RiskBusinessCase
{

    /// <summary>
    /// 
    /// </summary>
    public class CounterpartyAssessmentDetail
    {
        /// <summary>
        /// severity.
        /// </summary>
        /// <value>severity.</value>
        public long? Id { get; set; }

        /// <summary>
        /// counterPartyId.
        /// </summary>
        /// <value>counterPartyId.</value>
        public long? CounterPartyId { get; set; }

        /// <summary>
        /// customerId.
        /// </summary>
        /// <value>customerId.</value>
        public ResourceIdentifier CustomerId { get; set; }

        /// <summary>
        /// assessmentDetail.
        /// </summary>
        /// <value>assessmentDetail.</value>
        public AssessmentDetail AssessmentDetail { get; set; }
    }
}
