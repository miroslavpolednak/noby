namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.LoanApplicationAssessment
{

    /// <summary>
    /// Counterparty Assessment Detail
    /// </summary>
    public class CounterpartyAssessmentDetail
    {
        /// <summary>
        /// Id
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// Counterparty id
        /// </summary>
        public long? CounterPartyId { get; set; }

        /// <summary>
        /// Customer id
        /// </summary>
        public ResourceIdentifier CustomerId { get; set; }

        /// <summary>
        /// Assessment Detail
        /// </summary>
        public AssessmentDetail AssessmentDetail { get; set; }
    }
}
